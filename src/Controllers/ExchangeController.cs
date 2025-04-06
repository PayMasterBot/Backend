using DataAccess;
using DataAccess.Interface;
using DataAccess.Repository;
using Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using System.Text.Json.Nodes;

namespace src.Controllers
{
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private IExchangeRepository _rep;

        public ExchangeController(PgPayContext ctx) : base()
        {
            _rep = new PgExchangeRepository(ctx);
        }

        [Route("/api/exchange/balance")]
        [HttpGet]
        public ActionResult<JsonObject> GetBalance([FromQuery] int userId)
        {
            var res = _rep.GetBalance(userId);
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/exchange/auth")]
        [HttpPost]
        public ActionResult MakeAuth([FromQuery] int userId, [FromBody] string token)
        {
            return _rep.ExchangeAuth(userId, token) ? Ok() : BadRequest();
        }
    }
}
