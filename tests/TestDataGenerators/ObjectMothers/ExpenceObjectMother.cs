using Model;

namespace TestDataGenerators.ObjectMothers
{
	public static class ExpenceObjectMother
	{
        private static int _nextId = 1;

        public static Expence CreateExpence()
        {
            User _user = UserObjectMother.CreateUser();
            return new Expence
            {
                Id = _nextId++,
                UserId = _user.Id,
                User = _user,
                CategoryId = 1,
                Category = ExpenceCategoryObjectMother.CreateExpenceCategory(),
                Title = "Food",
                Price = 1500,
                Date = DateTime.UtcNow
            };
        }

        public static Expence CreateExpenceWithUser(User user)
        {
            return new Expence
            {
                Id = _nextId++,
                UserId = user.Id,
                User = user,
                CategoryId = 1,
                Category = ExpenceCategoryObjectMother.CreateExpenceCategory(),
                Title = "Transport",
                Price = 500,
                Date = DateTime.UtcNow
            };
        }

        public static Expence CreateExpenceWithCategory(ExpenceCategory category)
        {
            User _user = UserObjectMother.CreateUser();
            return new Expence
            {
                Id = _nextId++,
                UserId = _user.Id,
                User = _user,
                CategoryId = category.Id,
                Category = category,
                Title = category.Title + " Expense",
                Price = 1000,
                Date = DateTime.UtcNow
            };
        }

        public static Expence CreateExpenceWithTitle(string title)
        {
            User _user = UserObjectMother.CreateUser();
            return new Expence
            {
                Id = _nextId++,
                UserId = _user.Id,
                User = _user,
                CategoryId = 1,
                Category = ExpenceCategoryObjectMother.CreateExpenceCategory(),
                Title = title,
                Price = 1500,
                Date = DateTime.UtcNow
            };
        }

        public static Expence CreateExpenceWithPrice(int price)
        {
            return new Expence
            {
                Id = _nextId++,
                UserId = 1,
                User = UserObjectMother.CreateUser(),
                CategoryId = 1,
                Category = ExpenceCategoryObjectMother.CreateExpenceCategory(),
                Title = "Custom Price Expense",
                Price = price,
                Date = DateTime.UtcNow
            };
        }

        public static Expence CreateExpenceWithDate(DateTime date)
        {
            User _user = UserObjectMother.CreateUser();
            return new Expence
            {
                Id = _nextId++,
                UserId = _user.Id,
                User = _user,
                CategoryId = 1,
                Category = ExpenceCategoryObjectMother.CreateExpenceCategory(),
                Title = "Historical Expense",
                Price = 750,
                Date = date
            };
        }

        public static void ResetIdSequence()
        {
            _nextId = 1;
        }
    }
}

