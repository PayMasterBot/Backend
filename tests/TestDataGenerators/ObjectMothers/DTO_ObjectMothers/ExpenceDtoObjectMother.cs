using System;
using Model.DTO;

namespace TestDataGenerators.ObjectMothers.DTO_ObjectMothers
{
	public class ExpenceDtoObjectMother
	{
        public static ExpenceDto CreateExpenceDto()
        {
            return new ExpenceDto
            {
                UserId = 1,
                CatId = 1,
                Date = DateTime.UtcNow,
                Price = 1000,
                Title = "TestExpenceDto1"
            };
        }

        public static ExpenceDto CreateExpenceDtoWithUser(int userId)
        {
            return new ExpenceDto
            {
                UserId = userId,
                CatId = 1,
                Date = DateTime.UtcNow,
                Price = 1500,
                Title = "TestExpenceDto2"
            };
        }

        public static ExpenceDto CreateExpenceDtoWithCategory(int categoryId)
        {
            return new ExpenceDto
            {
                UserId = 1,
                CatId = categoryId,
                Date = DateTime.UtcNow,
                Price = 2000,
                Title = $"Category {categoryId} Expense"
            };
        }

        public static ExpenceDto CreateExpenceDtoWithDate(DateTime date)
        {
            return new ExpenceDto
            {
                UserId = 1,
                CatId = 1,
                Date = date,
                Price = 750,
                Title = "TestExpenceDto3"
            };
        }

        public static List<ExpenceDto> CreateExpenceDtoList(int count = 3)
        {
            var expenses = new List<ExpenceDto>();
            for (int i = 1; i <= count; i++)
            {
                expenses.Add(new ExpenceDto
                {
                    UserId = i,
                    CatId = i,
                    Date = DateTime.UtcNow.AddDays(-i),
                    Price = 100 * i,
                    Title = $"Expense #{i}"
                });
            }
            return expenses;
        }
    }
}

