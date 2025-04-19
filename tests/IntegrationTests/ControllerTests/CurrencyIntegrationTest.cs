using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace IntegrationTests.ControllerTests
{
    [TestFixture]
    [AllureNUnit]
    [Category("Integration")]
    public class CurrencyIntegrationTest
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

            var exchRates = new List<ExchangeRateSubscription>
            {
                ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrenciesUser(users[0], "RUB", "EUR"),
                ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrenciesUser(users[0], "USD", "EUR"),
                ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrenciesUser(users[1], "USD", "CAD"),
                ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrenciesUser(users[2], "USD", "EUR"),
                ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrenciesUser(users[2], "RUB", "GBP"),
            };

            context.Users.AddRange(users);
            context.ExchangeSubs.AddRange(exchRates);
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
        public async Task GetRate_Positive()
        {
            // Act
            var response = await _client.GetAsync("/api/currency-pair/rate?Cur1=USD&Cur2=EUR");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var rate = await response.Content.ReadFromJsonAsync<double>();
            Assert.Greater(rate, 0);
        }

        //[Test]
        public async Task GetRate_Negative()
        {
            // Act
            var response = await _client.GetAsync("/api/currency-pair/rate?Cur1=XXX&Cur2=YYY");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task AllCurrencyPairs_Positive()
        {
            // Act
            var response = await _client.GetAsync($"/api/currency-pair?userId={_testUser.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var subscriptions = await response.Content.ReadFromJsonAsync<List<ExchangeRateSubscription>>();
            Assert.AreEqual(2, subscriptions.Count);
            Assert.AreEqual("RUB", subscriptions[0].Currency1);
            Assert.AreEqual("EUR", subscriptions[0].Currency2);
        }

        [Test]
        public async Task AllCurrencyPairs_Negative_InvalidUser()
        {
            // Act
            var response = await _client.GetAsync("/api/currency-pair?userId=999");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var subscriptions = await response.Content.ReadFromJsonAsync<List<ExchangeRateSubscription>>();
            Assert.IsEmpty(subscriptions);
        }

        [Test]
        public async Task AddCurrencyPair_Positive()
        {
            // Arrange
            var newPair = new { Cur1 = "EUR", Cur2 = "GBP" };

            // Act
            var response = await _client.PostAsJsonAsync(
                $"/api/currency-pair?userId={_testUser.Id}",
                newPair);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var subscription = await response.Content.ReadFromJsonAsync<ExchangeRateSubscription>();
            Assert.AreEqual(_testUser.Id, subscription.UserId);
            Assert.AreEqual("EUR", subscription.Currency1);
            Assert.AreEqual("GBP", subscription.Currency2);
        }

        [Test]
        public async Task AddCurrencyPair_Negative_DuplicatePair()
        {
            // Arrange
            var duplicatePair = ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("USD", "EUR");

            // Act
            var response = await _client.PostAsJsonAsync(
                $"/api/currency-pair?userId={_testUser.Id}",
                duplicatePair);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task DeleteCurrencyPair_Positive()
        {
            // Arrange
            var pairToDelete = new { Cur1 = "USD", Cur2 = "EUR" };

            var response_pairs = await _client.GetAsync($"/api/currency-pair?userId={_testUser.Id}");
            var subscriptions_old = await response_pairs.Content.ReadFromJsonAsync<List<ExchangeRateSubscription>>();
            var old_count = subscriptions_old.Count;

            // Act
            var response = await _client.DeleteAsJsonAsync(
                $"/api/currency-pair?userId={_testUser.Id}",
                pairToDelete);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var subscriptionsResponse = await _client.GetAsync($"/api/currency-pair?userId={_testUser.Id}");
            var subscriptions = await subscriptionsResponse.Content.ReadFromJsonAsync<List<ExchangeRateSubscription>>();
            Assert.AreEqual(old_count-1, subscriptions.Count);
        }

        [Test]
        public async Task DeleteCurrencyPair_Negative()
        {
            // Arrange
            var invalidPair = ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("XXX", "YYY");

            // Act
            var response = await _client.DeleteAsJsonAsync(
                $"/api/currency-pair?userId={_testUser.Id}",
                invalidPair);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        //[Test]
        public async Task GetReport_Positive()
        {
            // Arrange
            var validPair = ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("USD", "EUR");

            // Act
            var response = await _client.GetAsJsonAsync(
                "/api/currency-pair/report",
                validPair);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var report = await response.Content.ReadFromJsonAsync<ExchangeRateSubscription>();
            Assert.IsNotNull(report);
        }

        [Test]
        public async Task GetReport_Negative()
        {
            // Arrange
            var invalidPair = ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("XXX", "YYY");

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/currency-pair/report",
                invalidPair);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<T>(
            this HttpClient client, string requestUri, T data)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
            {
                Content = JsonContent.Create(data)
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> GetAsJsonAsync<T>(
            this HttpClient client, string requestUri, T data)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri)
            {
                Content = JsonContent.Create(data)
            };
            return await client.SendAsync(request);
        }
    }
}

