using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ICurrencyRepository
    {
        public double GetExchangeRate(string cur1, string cur2);
        public ExchangeRateSubscription? AddCurrencyPairSubscription(ExchangeRateSubscription sub);
        public ICollection<ExchangeRateSubscription> GetCurrencyPairs(int userId);
        public bool DeleteCurrencyPairSubscription(ExchangeRateSubscription sub);
        public JsonObject? GetReport(ExchangeRateSubscription sub);
    }
}
