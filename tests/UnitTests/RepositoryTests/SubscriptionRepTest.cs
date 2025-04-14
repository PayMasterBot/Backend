using Microsoft.EntityFrameworkCore;
using System.Linq;
using DataAccess;
using DataAccess.Repository;
using Model;

namespace UnitTests.RepositoryTests
{
    [TestFixture]
    [AllureNUnit]
    [Category("Unit")]
    public class SubscriptionRepTest
    {
        private PgPayContext _context;
        private PgSubscriptionRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PgPayContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new PgPayContext(options);
            _repository = new PgSubscriptionRepository(_context);

            var subs = new List<Subscription>
            {
                new SubscriptionBuilder()
                .WithId(10)
                .WithUserId(1)
                .WithPrice(100)
                .WithTitle("Test1")
                .Build()
                ,
                new SubscriptionBuilder()
                .WithId(20)
                .WithUserId(1)
                .WithPrice(912)
                .WithTitle("Test2")
                .Build()
                ,
                new SubscriptionBuilder()
                .WithId(30)
                .WithUserId(2)
                .WithPrice(1000)
                .WithTitle("Test3")
                .Build()
                ,
            };
            _context.Subscriptions.AddRange(subs);
            _context.SaveChanges();
        }

        [Test]
        public void AddSubscription_Positive()
        {
            // Arrange
            var sub = SubscriptionObjectMother.CreateSubscription();

            // Act
            var result = _repository.AddSubscription(sub);

            // Assert
            Assert.NotNull(result);
        }


        [Test]
        public void DeleteSubscription_Positive()
        {
            // Act
            var result = _repository.DeleteSubscription(10);
            var fromDb = _context.Subscriptions.Find(10);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(fromDb);
        }

        [Test]
        public void DeleteSubscription_Negative()
        {
            // Act
            var result = _repository.DeleteSubscription(99);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void FindSubscription_Positive()
        {
            // Act
            var result = _repository.GetSubscription(10);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public void FindSubscription_Negative()
        {
            // Act
            var result = _repository.GetSubscription(99);

            // Assert
            Assert.IsNull(result);
        }


        [TearDown]
        public void TearDown()
        {
            _context?.Database?.EnsureDeleted();
            _context?.Dispose();
        }
    }
}