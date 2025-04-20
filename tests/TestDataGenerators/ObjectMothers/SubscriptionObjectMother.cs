using Model;

namespace TestDataGenerators.ObjectMothers
{
	public static class SubscriptionObjectMother
	{
        private static int _nextId = 1;

        public static Subscription CreateSubscription()
        {
            var user = UserObjectMother.CreateUser();
            return new Subscription
            {
                Id = _nextId++,
                UserId = user.Id,
                User = user,
                Title = "TestSubscription1",
                Price = 1000
            };
        }

        public static Subscription CreateSubscriptionWithUser(User user)
        {
            return new Subscription
            {
                Id = _nextId++,
                UserId = user.Id,
                User = user,
                Title = "TestSubscription3",
                Price = 2000
            };
        }

        public static Subscription CreateSubscriptionWithTitle(string title)
        {
            return new Subscription
            {
                Id = _nextId++,
                UserId = 1,
                User = UserObjectMother.CreateUser(),
                Title = title,
                Price = 1500
            };
        }

        public static Subscription CreateSubscriptionWithPrice(int price)
        {
            var user = UserObjectMother.CreateUser();
            return new Subscription
            {
                Id = _nextId++,
                UserId = user.Id,
                User = user,
                Title = "TestSubscription4",
                Price = price
            };
        }

        public static void ResetIdSequence()
        {
            _nextId = 1;
        }
    }
}

