
namespace IntegrationTests.ControllerTests
{
    [TestFixture]
    [AllureNUnit]
    [Category("Integration")]
    public class SubscriptionIntegrationTest
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

            var subs = new List<Subscription>
            {
                new SubscriptionBuilder()
                .WithId(10)
                .WithUser(_testUser)
                .WithPrice(100)
                .WithTitle("Test1")
                .Build()
                ,
                new SubscriptionBuilder()
                .WithId(20)
                .WithUserId(2)
                .WithPrice(900)
                .WithTitle("Test2")
                .Build()
                ,
                new SubscriptionBuilder()
                .WithId(30)
                .WithUserId(2)
                .WithPrice(1000)
                .WithTitle("Test3")
                .Build()
            };
            context.Subscriptions.AddRange(subs);

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
        public async Task AddSubscription_Positive()
        {
            // Arrange
            SubscriptionObjectMother.ResetIdSequence();
            var newSub = SubscriptionObjectMother
                .CreateSubscriptionWithUser(_testUser);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/subscription?userId=1", newSub);
            var result = await response.Content.ReadFromJsonAsync<Subscription>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task AllSubscriptions_Positive_UserSubscriptions()
        {
            // Act
            var response = await _client.GetAsync("/api/subscription?userId=1");
            var result = await response.Content.ReadFromJsonAsync<List<Subscription>>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.All(s => s.UserId == 1));
        }

        [Test]
        public async Task GetSubscription_Positive_SpecificSubscription()
        {
            // Act
            var response = await _client.GetAsync("/api/subscription/10");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetSubscription_Negative()
        {
            // Act
            var response = await _client.GetAsync("/api/subscription/99");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task DeleteSubscription_Positive()
        {
            // Act
            var deleteResponse = await _client.DeleteAsync("/api/subscription/10");
            var getResponse = await _client.GetAsync("/api/subscription/10");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, getResponse.StatusCode);
        }

        [Test]
        public async Task DeleteSubscription_Negative()
        {
            // Act
            var response = await _client.DeleteAsync("/api/subscription/99");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

