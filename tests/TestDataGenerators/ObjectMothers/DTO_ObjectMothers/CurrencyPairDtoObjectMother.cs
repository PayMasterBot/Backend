using System;
using Model.DTO;

namespace TestDataGenerators.ObjectMothers.DTO_ObjectMothers
{
	public class CurrencyPairDtoObjectMother
	{
        public static CurrencyPairDto CreateCurrencyPair()
        {
            return new CurrencyPairDto
            {
                Cur1 = "USD",
                Cur2 = "EUR"
            };
        }

        public static CurrencyPairDto CreateCurrencyPairWithCurrencies(string currency1, string currency2)
        {
            return new CurrencyPairDto
            {
                Cur1 = currency1,
                Cur2 = currency2
            };
        }
    }
}

