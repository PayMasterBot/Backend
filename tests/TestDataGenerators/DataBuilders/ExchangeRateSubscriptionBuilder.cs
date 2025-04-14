using System;
using Model;

namespace TestDataGenerators.DataBuilders
{
	public class ExchangeRateSubscriptionBuilder
	{
		private ExchangeRateSubscription _exchangeRateSubscription = new ExchangeRateSubscription();

        public ExchangeRateSubscriptionBuilder WithUserId(int userId)
		{
			_exchangeRateSubscription.UserId = userId;
			return this;
        }

        public ExchangeRateSubscriptionBuilder WithUser(User user)
        {
            _exchangeRateSubscription.User = user;
            _exchangeRateSubscription.UserId = user.Id;
            return this;
        }

        public ExchangeRateSubscriptionBuilder WithCurrency1(string currency1)
        {
            _exchangeRateSubscription.Currency1 = currency1;
            return this;
        }

        public ExchangeRateSubscriptionBuilder WithCurrency2(string currency2)
        {
            _exchangeRateSubscription.Currency2 = currency2;
            return this;
        }

        public ExchangeRateSubscription Build()
        {
            return _exchangeRateSubscription;
        }
    }
}

