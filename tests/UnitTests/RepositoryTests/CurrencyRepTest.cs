using System;
using DataAccess;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Model;

namespace UnitTests.RepositoryTests
{
    [TestFixture]
    [AllureNUnit]
    [Category("Unit")]
    public class CurrencyRepTest
    {
        private PgPayContext _context;
        private PgCurrencyRepository _repository;

        private ExchangeRateSubscription _rateToDelete;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PgPayContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PgPayContext(options);
            _repository = new PgCurrencyRepository(_context);

            _rateToDelete = ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("RUB", "GBP");

            var exchRates = new List<ExchangeRateSubscription>
            {
                ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("RUB", "EUR"),
                ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("USD", "CAD"),
                _rateToDelete,
            };
            _context.ExchangeSubs.AddRange(exchRates);

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddCurrencyPairSubscription_Positive()
        {
            // Arrange
            var exchRate = ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscription();

            // Act
            var result = _repository.AddCurrencyPairSubscription(exchRate);

            // Assert
            Assert.NotNull(result);
        }


        [Test]
        public void DeleteCurrencyPairSubscription_Positive()
        {
            // Act
            var result = _repository.DeleteCurrencyPairSubscription(_rateToDelete);

            // Assert
            Assert.AreEqual(true, result);
            Assert.IsNull(_context.Subscriptions.Find(_rateToDelete.UserId));
        }


        [Test]
        public void DeleteCurrencyPairSubscription_Negative()
        {
            // Arrange
            var exchRate = ExchangeRateSubscriptionObjectMother
                .CreateExchangeRateSubscriptionWithCurrencies("NULL", "NULL");

            // Act
            var result = _repository.DeleteCurrencyPairSubscription(exchRate);

            // Assert
            Assert.AreEqual(false, result);
        }


    }

}