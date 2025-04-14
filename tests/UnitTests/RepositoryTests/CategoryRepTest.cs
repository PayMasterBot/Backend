using System;
using System.Text.Json.Nodes;
using DataAccess;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Model;
using Newtonsoft.Json.Linq;

namespace UnitTests.RepositoryTests
{
    [TestFixture]
    [AllureNUnit]
    [Category("Unit")]
    public class CategoryRepTest
    {
        private PgPayContext _context;
        private PgCategoryRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PgPayContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new PgPayContext(options);
            _repository = new PgCategoryRepository(_context);

            UserObjectMother.ResetIdSequence();
            var users = new List<User>
            {
                UserObjectMother.CreateUserWithToken("userToken1"),
                UserObjectMother.CreateUserWithToken("userToken2"),
                UserObjectMother.CreateUserWithToken("userToken3"),
            };
            _context.Users.AddRange(users);

            ExpenceCategoryObjectMother.ResetIdSequence();
            var categs = new List<ExpenceCategory>
            {
                ExpenceCategoryObjectMother.CreateExpenceCategoryWithTitle("TestTitle1"),
                ExpenceCategoryObjectMother.CreateExpenceCategoryWithTitle("TestTitle2"),
                ExpenceCategoryObjectMother.CreateExpenceCategoryWithTitle("TestTitle3"),
            };
            _context.ExpenceCategories.AddRange(categs);

            _context.SaveChanges();
        }

        [Test]
        public void AddCategory_Positive_Creating()
        {
            // Arrange
            var user = _context.Users.Find(1);
            var newCategory = new ExpenceCategoryBuilder()
            .WithId(100)
            .WithTitle("Transport")
            .Build();

            // Act
            var result = _repository.AddCategory(newCategory, user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Id);
            Assert.IsTrue(user.ExpenceCategories.Any(c => c.Id == 100));
            Assert.IsTrue(_context.ExpenceCategories.Any(c => c.Id == 100));
        }

        [Test]
        public void AddCategory_Positive_Linking()
        {
            // Arrange
            var user = _context.Users.Find(1);
            var newCategory = new ExpenceCategoryBuilder()
            .WithId(1)
            .WithTitle("TestTitle1")
            .Build();

            // Act
            var result = _repository.AddCategory(newCategory, user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(user, result.Users.FirstOrDefault(u => u.Id == user.Id));
        }

        [Test]
        public void AddExpense_Positive()
        {
            // Arrange
            var expense = ExpenceObjectMother.CreateExpence();

            // Act
            var result = _repository.AddExpense(expense);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, _context.Expences.Count());
        }

        [Test]
        public void DeleteCategory_Positive()
        {
            // Arrange
            var user = _context.Users.Find(1);
            _repository.AddCategory(_context.ExpenceCategories.Find(1), user);
            // Act
            var result = _repository.DeleteCategory(1, 1);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(user.ExpenceCategories.Any(c => c.Id == 1));
        }

        [Test]
        public void GetCategories_UserCategories()
        {
            // Arrange
            var user = _context.Users.Find(1);
            _repository.AddCategory(_context.ExpenceCategories.Find(1), user);

            // Act
            var result = _repository.GetCategories(1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("TestTitle1", result.First().Title);
        }

        [Test]
        public void GetCategory_SpecificCategory()
        {
            // Arrange
            var user = _context.Users.Find(1);
            _repository.AddCategory(_context.ExpenceCategories.Find(2), user);

            // Act
            var result = _repository.GetCategory(2, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("TestTitle2", result.Title);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database?.EnsureDeleted();
            _context?.Dispose();
        }
    }
}

