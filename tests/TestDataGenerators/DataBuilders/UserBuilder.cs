using Model;

namespace TestDataGenerators.DataBuilders
{
	public class UserBuilder
	{
		private User _user = new User();


		public UserBuilder WithId(int id)
		{
			_user.Id = id;
			return this;
		}

        public UserBuilder WithToken(string token)
        {
            _user.Token = token;
            return this;
        }

        public UserBuilder WithExpenceCategories(ICollection<ExpenceCategory> expenceCategories)
        {
            _user.ExpenceCategories = expenceCategories;
            return this;
        }

        public UserBuilder WithExpences(ICollection<Expence> expences)
        {
            _user.Expences= expences;
            return this;
        }

        public UserBuilder WithSubscriptions(ICollection<Subscription> subscriptions)
        {
            _user.Subscriptions = subscriptions;
            return this;
        }

        public UserBuilder WithExchangeRates(ICollection<ExchangeRateSubscription> exchangeRates)
        {
            _user.ExchangeRates = exchangeRates;
            return this;
        }


        public User Build()
        {
            return _user;
        }
    }
}

