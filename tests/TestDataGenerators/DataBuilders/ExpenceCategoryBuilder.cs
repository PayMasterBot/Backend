using Model;

namespace TestDataGenerators.DataBuilders
{
	public class ExpenceCategoryBuilder
	{
		private ExpenceCategory _expenceCategory = new ExpenceCategory();

        public ExpenceCategoryBuilder WithId(int id)
		{
			_expenceCategory.Id = id;
			return this;
        }

        public ExpenceCategoryBuilder WithTitle(string title)
        {
            _expenceCategory.Title = title;
            return this;
        }

        public ExpenceCategoryBuilder WithUsers(ICollection<User> users)
        {
            _expenceCategory.Users = users;
            return this;
        }

        public ExpenceCategoryBuilder WithExpences(ICollection<Expence> expences)
        {
            _expenceCategory.Expences = expences;
            return this;
        }

        public ExpenceCategory Build()
        {
            return _expenceCategory;
        }
    }
}

