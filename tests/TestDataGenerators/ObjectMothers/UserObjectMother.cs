using Model;

namespace TestDataGenerators.ObjectMothers
{
	public static class UserObjectMother
	{
        private static int _nextId = 1;

        public static User CreateUser()
		{
            return new User
            {
                Id = _nextId++,
                Token = "testToken-12345",
                ExpenceCategories = new List<ExpenceCategory>(),
                Expences = new List<Expence>(),
                Subscriptions = new List<Subscription>(),
                ExchangeRates = new List<ExchangeRateSubscription>()
            };
        }

        public static User CreateUserWithCollections(
            ICollection<ExpenceCategory> categories = null,
            ICollection<Expence> expenses = null,
            ICollection<Subscription> subscriptions = null,
            ICollection<ExchangeRateSubscription> exchangeRates = null)
        {
            return new User
            {
                Id = _nextId++,
                Token = "testToken-23456",
                ExpenceCategories = categories ?? new List<ExpenceCategory>(),
                Expences = expenses ?? new List<Expence>(),
                Subscriptions = subscriptions ?? new List<Subscription>(),
                ExchangeRates = exchangeRates ?? new List<ExchangeRateSubscription>()
            };
        }

        public static User CreateUserWithToken(string token)
        {
            return new User
            {
                Id = _nextId++,
                Token = token,
                ExpenceCategories = new List<ExpenceCategory>(),
                Expences = new List<Expence>(),
                Subscriptions = new List<Subscription>(),
                ExchangeRates = new List<ExchangeRateSubscription>()
            };
        }

        public static User CreateUserWithExpenseCategories(ICollection<ExpenceCategory> categories)
        {
            return new User
            {
                Id = _nextId++,
                Token = "testToken-45678",
                ExpenceCategories = categories ?? new List<ExpenceCategory>(),
                Expences = new List<Expence>(),
                Subscriptions = new List<Subscription>(),
                ExchangeRates = new List<ExchangeRateSubscription>()
            };
        }

        public static User CreateUserWithExpenses(ICollection<Expence> expenses)
        {
            return new User
            {
                Id = _nextId++,
                Token = "testToken-56789",
                ExpenceCategories = new List<ExpenceCategory>(),
                Expences = expenses ?? new List<Expence>(),
                Subscriptions = new List<Subscription>(),
                ExchangeRates = new List<ExchangeRateSubscription>()
            };
        }

        public static User CreateUserWithSubscriptions(ICollection<Subscription> subscriptions)
        {
            return new User
            {
                Id = _nextId++,
                Token = "testToken-67890",
                ExpenceCategories = new List<ExpenceCategory>(),
                Expences = new List<Expence>(),
                Subscriptions = subscriptions ?? new List<Subscription>(),
                ExchangeRates = new List<ExchangeRateSubscription>()
            };
        }

        public static User CreateUserWithExchangeRates(ICollection<ExchangeRateSubscription> exchangeRates)
        {
            return new User
            {
                Id = _nextId++,
                Token = "testToken-78901",
                ExpenceCategories = new List<ExpenceCategory>(),
                Expences = new List<Expence>(),
                Subscriptions = new List<Subscription>(),
                ExchangeRates = exchangeRates ?? new List<ExchangeRateSubscription>()
            };
        }


        public static void ResetIdSequence()
        {
            _nextId = 1;
        }
    }
}

