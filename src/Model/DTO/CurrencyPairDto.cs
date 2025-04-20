using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class CurrencyPairDto
    {
        public string Cur1 { get; set; }
        public string Cur2 { get; set; }

        public static implicit operator ExchangeRateSubscription(CurrencyPairDto pair)
        {
            return new ExchangeRateSubscription
            {
                Currency1 = pair.Cur1,
                Currency2 = pair.Cur2
            };
        }

        public static implicit operator CurrencyPairDto(ExchangeRateSubscription rate)
        {
            return new CurrencyPairDto
            {
                Cur1 = rate.Currency1,
                Cur2 = rate.Currency2
            };
        }
    }
}
