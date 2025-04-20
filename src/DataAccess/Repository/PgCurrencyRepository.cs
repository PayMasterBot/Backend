using DataAccess.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
            double res = 0.0;
            using (var cli = new HttpClient())
            {
                var data = cli.GetAsync($"https://api.binance.com/api/v3/ticker/price?symbol={cur1}{cur2}");
                data.Wait();
                using (var doc = JsonDocument.Parse(data.Result.Content.ReadAsStream()))
                {
                    var tmp = doc.RootElement.GetProperty("price").GetString();
                    res = Convert.ToDouble(tmp, System.Globalization.CultureInfo.InvariantCulture);
                }
            };
            return res;
        }

        public JsonObject? GetReport(ExchangeRateSubscription sub)
        {
            try
            {
                using (var cli = new HttpClient())
                {
                    var data = cli.GetAsync($"https://api.binance.com/api/v3/klines?symbol={sub.Currency1}{sub.Currency2}&interval=1d&limit=100");
                    data.Wait();
                    var dataString = data.Result.Content.ReadAsStringAsync();
                    dataString.Wait();
                    var arr = JsonObject.Parse(dataString.Result).AsArray();
                    JsonObject res = new();
                    res.Add("report", arr);
                    return res;
                };
            }
            catch { }
            return null;
        }
    }
}
