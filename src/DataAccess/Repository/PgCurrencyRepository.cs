using DataAccess.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PgCurrencyRepository : ICurrencyRepository
    {
        private PgPayContext _ctx;
        public PgCurrencyRepository(PgPayContext ctx)
        {
            _ctx = ctx;
        }

        public ExchangeRateSubscription? AddCurrencyPairSubscription(ExchangeRateSubscription sub)
        {
            try
            {
                _ctx.ExchangeSubs.Add(sub);
                _ctx.SaveChanges();
                return sub;
            }
            catch { }
            return null;
        }

        public bool DeleteCurrencyPairSubscription(ExchangeRateSubscription sub)
        {
            try
            {
                _ctx.ExchangeSubs.Remove(sub);
                _ctx.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }

        public ICollection<ExchangeRateSubscription> GetCurrencyPairs(int userId)
        {
            try
            {
                return _ctx.ExchangeSubs.Where(e => e.UserId == userId).AsEnumerable().ToList();
            }
            catch { }
            return new List<ExchangeRateSubscription>();
        }

        public double GetExchangeRate(string cur1, string cur2)
        {
            return 0.0;
        }

        public JsonObject? GetReport(ExchangeRateSubscription sub)
        {
            return null;
        }
    }
}
