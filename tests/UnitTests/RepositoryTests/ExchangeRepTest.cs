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
    public class ExchangeRepTest
    {
        private PgPayContext _context;
        private PgExchangeRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PgPayContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PgPayContext(options);
            _repository = new PgExchangeRepository(_context);

            UserObjectMother.ResetIdSequence();
            var users = new List<User>
            {
                UserObjectMother.CreateUserWithToken("userToken1"),
                UserObjectMother.CreateUserWithToken("userToken2"),
                UserObjectMother.CreateUserWithToken("userToken3"),
            };

            _context.Users.AddRange(users);

            // Добавляем тестовые курсы валют
            _context.ExchangeSubs.AddRange(
                new ExchangeRateSubscription { UserId = 1, Currency1 = "USD", Currency2 = "EUR" },
                new ExchangeRateSubscription { UserId = 1, Currency1 = "EUR", Currency2 = "GBP" }
            );

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


    }
}

