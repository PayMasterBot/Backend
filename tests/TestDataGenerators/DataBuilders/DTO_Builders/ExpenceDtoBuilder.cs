using System;
using Model.DTO;

namespace TestDataGenerators.DataBuilders.DTO_Builders
{
	public class ExpenceDtoBuilder
	{
        private ExpenceDto _expenceDto = new ExpenceDto();

        public ExpenceDtoBuilder WithUserId(int userId)
        {
            _expenceDto.UserId = userId;
            return this;
        }

        public ExpenceDtoBuilder WithCatId(int catId)
        {
            _expenceDto.CatId = catId;
            return this;
        }

        public ExpenceDtoBuilder WithDate(DateTime date)
        {
            _expenceDto.Date = date;
            return this;
        }

        public ExpenceDtoBuilder WithPrice(int price)
        {
            _expenceDto.Price = price;
            return this;
        }

        public ExpenceDtoBuilder WithTitle(string title)
        {
            _expenceDto.Title = title;
            return this;
        }

        public ExpenceDto Build()
        {
            return _expenceDto;
        }
    }
}

