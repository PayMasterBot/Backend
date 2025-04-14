using Model;

namespace TestDataGenerators.ObjectMothers
{
	public static class ExchangeRateSubscriptionObjectMother
	{
		public static ExchangeRateSubscription CreateExchangeRateSubscription()
		{
            User _user = UserObjectMother.CreateUser();
            return new ExchangeRateSubscription
            {
                UserId = _user.Id,
                User = _user,
                Currency1 = "USD",
                Currency2 = "EUR"
            };
        }

        public static ExchangeRateSubscription CreateExchangeRateSubscriptionWithUser(User user)
        {
            return new ExchangeRateSubscription
            {
                UserId = user.Id,
                User = user,
                Currency1 = "RUB",
                Currency2 = "USD"
            };
        }

        public static ExchangeRateSubscription CreateExchangeRateSubscriptionWithCurrencies(string currency1, string currency2)
        {
            User _user = UserObjectMother.CreateUser();
            return new ExchangeRateSubscription
            {
                UserId = _user.Id,
                User = _user,
                Currency1 = currency1,
                Currency2 = currency2
            };
        }
    }
}

