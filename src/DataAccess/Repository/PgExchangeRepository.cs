using DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PgExchangeRepository : IExchangeRepository
    {
        private PgPayContext _ctx;

        public PgExchangeRepository(PgPayContext ctx)
        {
            _ctx = ctx;
        }

        public bool ExchangeAuth(int userId, string token)
        {
            //
            return false;
        }

        public JsonObject? GetBalance(int userId)
        {
            //https://api.binance.com/api/v3/account
            // => "balances"
            using (var cli = new HttpClient())
            {
                var data = cli.GetAsync($"https://api.binance.com/api/v3/account");
                data.Wait();
                using (var doc = JsonDocument.Parse(data.Result.Content.ReadAsStream()))
                {
                    doc.RootElement.GetProperty("price").GetDouble();
                }
            };
            return null;
        }
    }
}
