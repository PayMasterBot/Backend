using Model;

namespace TestDataGenerators.ObjectMothers
{
	public static class ExpenceCategoryObjectMother
    {
        private static int _nextId = 1;

        public static ExpenceCategory CreateExpenceCategory()
		{
            return new ExpenceCategory
            {
                Id = _nextId++,
                Title = "TestCategory1",
                Users = new List<User>(),
                Expences = new List<Expence>()
            };
        }

        public static ExpenceCategory CreateExpenceCategoryWithTitle(string title)
        {
            return new ExpenceCategory
            {
                Id = _nextId++,
                Title = title,
                Users = new List<User>(),
                Expences = new List<Expence>()
            };
        }

        public static ExpenceCategory CreateExpenceCategoryWithUsers(ICollection<User> users)
        {
            return new ExpenceCategory
            {
                Id = _nextId++,
                Title = "TestCategory2",
                Users = new List<User>(users),
                Expences = new List<Expence>()
            };
        }

        public static ExpenceCategory CreateExpenceCategoryWithExpences(ICollection<Expence> expences)
        {
            return new ExpenceCategory
            {
                Id = _nextId++,
                Title = "TestCategory3",
                Users = new List<User>(),
                Expences = new List<Expence>(expences)
            };
        }

        public static void ResetIdSequence()
        {
            _nextId = 1;
        }
    }
}

