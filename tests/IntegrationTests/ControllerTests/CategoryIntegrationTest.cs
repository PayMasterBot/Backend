using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace IntegrationTests.ControllerTests
{
	public class CategoryIntegrationTest
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private User _testUser;
        private ExpenceCategory _testCategory;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<PgPayContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.Configure<HttpsRedirectionOptions>(options =>
                        {
                            options.HttpsPort = 5001;
                        });

                        services.AddDbContext<PgPayContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            _client = _factory.CreateClient();

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PgPayContext>();

            ExpenceCategoryObjectMother.ResetIdSequence();
            _testCategory = ExpenceCategoryObjectMother.CreateExpenceCategoryWithTitle("TestTitle1");
            var categs = new List<ExpenceCategory>
            {
                _testCategory,
                ExpenceCategoryObjectMother.CreateExpenceCategoryWithTitle("TestTitle2"),
                ExpenceCategoryObjectMother.CreateExpenceCategoryWithTitle("TestTitle3"),
            };

            context.ExpenceCategories.AddRange(categs);


            UserObjectMother.ResetIdSequence();
            _testUser = UserObjectMother.CreateUserWithToken("userToken1");
            _testUser.ExpenceCategories = new ExpenceCategory[] { _testCategory };

            var users = new List<User>
            {
                _testUser,
                UserObjectMother.CreateUserWithToken("userToken2"),
                UserObjectMother.CreateUserWithToken("userToken3"),
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PgPayContext>();
                context.Database.EnsureDeleted();
            }

            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task AddCategory_Positive()
        {
            // Arrange
            var newCategory = ExpenceCategoryObjectMother.CreateExpenceCategoryWithTitle("Transport");

            // Act
            var response = await _client.PostAsJsonAsync(
                $"/api/category?userId={_testUser.Id}",
                newCategory);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task AllCategories_Positive()
        {
            // Arrange
            var userId = 2;

            // Act
            var response = await _client.GetAsync($"/api/category?userId={userId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var categories = await response.Content.ReadFromJsonAsync<List<ExpenceCategory>>();
            Assert.AreEqual(0, categories.Count);
        }

        [Test]
        public async Task DeleteCategory_Positive()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/category/1?userId={_testUser.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Проверяем, что категория удалена
            var checkResponse = await _client.GetAsync($"/api/category/1?userId={_testUser.Id}");
            Assert.AreEqual(HttpStatusCode.BadRequest, checkResponse.StatusCode);
        }

        //[Test]
        public async Task GetCategory_Positive()
        {
            // Act
            var response = await _client.GetAsync($"/api/category/1?userId={_testUser.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var category = await response.Content.ReadFromJsonAsync<ExpenceCategory>();
            Assert.AreEqual("TestTitle1", category.Title);
        }

        [Test]
        public async Task AddSpending_Positive()
        {
            // Arrange
            var newExpense = new { Title = "Lunch", Price = 500, Date = DateTime.Now };

            // Act
            var response = await _client.PostAsJsonAsync(
                $"/api/category/1/spending?userId={_testUser.Id}",
                newExpense);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var expense = await response.Content.ReadFromJsonAsync<Expence>();
            Assert.AreEqual("Lunch", expense.Title);
            Assert.AreEqual(1, expense.CategoryId);
        }
    }
}

