using DataAccess;
using DataAccess.Interface;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace src.Controllers
{
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private IExchangeRepository _rep;
        private ILogger<ExchangeController> _log;

        public ExchangeController(PgPayContext ctx, ILogger<ExchangeController> logger) : base()
        {
            _rep = new PgExchangeRepository(ctx);
            _log = logger;
        }

        [Route("/api/exchange/balance")]
        [HttpGet]
        public ActionResult<JsonObject> GetBalance([FromQuery] int userId)
        {
            var res = _rep.GetBalance(userId);
            if (res is null)
            {
                _log.LogError($"Can't get balance for user {userId}");
            }
            else
            {
                _log.LogInformation($"Successfull get balance for user {userId}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/exchange/auth")]
        [HttpPost]
        public ActionResult MakeAuth([FromQuery] int userId, [FromBody] string token)
        {
            if (_rep.ExchangeAuth(userId, token))
            {
                _log.LogInformation($"Successfull auth user {userId}");
                return Ok();
            }
            else
            {
                _log.LogError($"Can't auth user {userId}");
                return BadRequest();
            }
        }
    }
}
