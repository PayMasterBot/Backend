using DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool ExchangeAuth(string token)
        {
            return false;
        }

        public JsonObject? GetBalance(int userId)
        {
            return null;
        }
    }
}
