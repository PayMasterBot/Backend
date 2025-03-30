using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IExchangeRepository
    {
        public JsonObject? GetBalance(int userId);
        public bool ExchangeAuth(string token);
    }
}
