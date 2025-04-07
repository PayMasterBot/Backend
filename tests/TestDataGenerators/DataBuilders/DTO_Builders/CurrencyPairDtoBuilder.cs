using System;
using Model.DTO;

namespace TestDataGenerators.DataBuilders.DTO_Builders
{
	public class CurrencyPairDtoBuilder
	{
		private CurrencyPairDto _currencyPairDto = new CurrencyPairDto();

		public CurrencyPairDtoBuilder WithCur1(string cur1)
		{
			_currencyPairDto.Cur1 = cur1;
			return this;
		}

        public CurrencyPairDtoBuilder WithCur2(string cur2)
        {
            _currencyPairDto.Cur2 = cur2;
            return this;
        }

        public CurrencyPairDto Build()
		{
			return _currencyPairDto;
		}
    }
}

