
using Newtonsoft.Json.Linq;

namespace IntegrationTests.ControllerTests
{
    [TestFixture]
    [AllureNUnit]
    [Category("Integration")]
    public class ExchangeIntegrationTest
	{
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private User _testUser;

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

            // Инициализация тестовых данных
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PgPayContext>();

            UserObjectMother.ResetIdSequence();
            _testUser = UserObjectMother.CreateUserWithToken("userToken1");
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

        //[Test]
        public async Task GetBalance_Positive()
        {
            // Act
            var response = await _client.GetAsync($"/api/exchange/balance?userId={_testUser.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //[Test]
        public async Task MakeAuth_Positive()
        {
            // Act
            var response = await _client.PostAsJsonAsync(
                $"/api/exchange/auth?userId={_testUser.Id}",
                _testUser.Token);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task MakeAuth_Negative_InvalidToken()
        {
            // Arrange
            var invalidToken = "invalid_token";

            // Act
            var response = await _client.PostAsJsonAsync(
                $"/api/exchange/auth?userId={_testUser.Id}",
                invalidToken);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task MakeAuth_Negative()
        {
            // Arrange
            var token = "any_token";

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/exchange/auth?userId=999",
                token);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

