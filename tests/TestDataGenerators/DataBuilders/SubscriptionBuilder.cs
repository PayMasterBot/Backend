using Model;

namespace TestDataGenerators.DataBuilders
{
	public class SubscriptionBuilder
	{
		private Subscription _subscription = new Subscription();

        public SubscriptionBuilder WithId(int id)
		{
			_subscription.Id = id;
			return this;
        }

        public SubscriptionBuilder WithUserId(int userId)
        {
            _subscription.UserId = userId;
            return this;
        }

        public SubscriptionBuilder WithUser(User user)
        {
            _subscription.User = user;
            return this;
        }

        public SubscriptionBuilder WithTitle(string title)
        {
            _subscription.Title = title;
            return this;
        }

        public SubscriptionBuilder WithPrice(int price)
        {
            _subscription.Price = price;
            return this;
        }


        public Subscription Build()
        {
            return _subscription;
        }
    }
}

