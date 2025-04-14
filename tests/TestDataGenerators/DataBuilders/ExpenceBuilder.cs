using Model;

namespace TestDataGenerators.DataBuilders
{
	public class ExpenceBuilder
	{
		private Expence _expence = new Expence();

		public ExpenceBuilder WithId(int id)
		{
			_expence.Id = id;
            return this;
		}

        public ExpenceBuilder WithUserId(int userId)
        {
            _expence.UserId = userId;
            return this;
        }

        public ExpenceBuilder WithUser(User user)
        {
            _expence.User = user;
            return this;
        }

        public ExpenceBuilder WithCategoryId(int categoryId)
        {
            _expence.CategoryId = categoryId;
            return this;
        }

        public ExpenceBuilder WithCategory(ExpenceCategory category)
        {
            _expence.Category = category;
            return this;
        }

        public ExpenceBuilder WithTitle(string title)
        {
            _expence.Title = title;
            return this;
        }

        public ExpenceBuilder WithPrice(int price)
        {
            _expence.Price = price;
            return this;
        }

        public ExpenceBuilder WithDate(DateTime date)
        {
            _expence.Date = date;
            return this;
        }

        public Expence Build()
        {
            return _expence;
        }
    }
}

