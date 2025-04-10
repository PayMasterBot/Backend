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
    public class CurrencyController : ControllerBase
    {
        private ICurrencyRepository _rep;
        private ILogger<CurrencyController> _log;

        public CurrencyController(PgPayContext ctx, ILogger<CurrencyController> logger) : base()
        {
            _rep = new PgCurrencyRepository(ctx);
            _log = logger;
        }

        [Route("/api/currency-pair/rate")]
        [HttpGet]
        public ActionResult<double> GetRate([FromQuery] string Cur1, [FromQuery] string Cur2)
        {
            try
            {
                var res = _rep.GetExchangeRate(Cur1, Cur2);
                _log.LogError($"Exchange rate for currencies {Cur1} and {Cur2}: {res}");
                return Ok(res);
            }
            catch
            {
                _log.LogError($"Can't get exchange rate for currencies {Cur1} and {Cur2}");
                return BadRequest();
            }
        }

        [Route("/api/currency-pair")]
        [HttpGet]
        public ActionResult<ICollection<ExchangeRateSubscription>> AllCurrencyPairs([FromQuery] int userId)
        {
            _log.LogInformation($"Successfull get list of subscribed pairs for user {userId}");
            return Ok(_rep.GetCurrencyPairs(userId));
        }

        [Route("/api/currency-pair")]
        [HttpDelete]
        public ActionResult DeleteCurrencyPair([FromQuery] int userId, [FromBody] CurrencyPairDto pair)
        {
            ExchangeRateSubscription sub = pair;
            sub.UserId = userId;
            if (_rep.DeleteCurrencyPairSubscription(sub))
            {
                _log.LogInformation($"Successfull delete subscription on pair {pair.Cur1}-{pair.Cur2} for user {userId}");
                return Ok();
            }
            else
            {
                _log.LogInformation($"Can't delete subscription on pair {pair.Cur1}-{pair.Cur2} for user {userId}");
                return BadRequest();
            }
        }

        [Route("/api/currency-pair")]
        [HttpPost]
        public ActionResult<ExchangeRateSubscription> AddCurrencyPair([FromQuery] int userId, [FromBody] CurrencyPairDto pair)
        {
            ExchangeRateSubscription sub = pair;
            sub.UserId = userId;
            var res = _rep.AddCurrencyPairSubscription(sub);
            if (res is null)
            {
                _log.LogError($"Can't subscribe on pair {pair.Cur1}-{pair.Cur2} for user {userId}");
            }
            else
            {
                _log.LogInformation($"Successfull subscribe on pair {pair.Cur1}-{pair.Cur2} for user {userId}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/currency-pair/report")]
        [HttpGet]
        public ActionResult<ExchangeRateSubscription> GetReport([FromBody] CurrencyPairDto pair)
        {
            var res = _rep.GetReport(pair);
            if (res is null)
            {
                _log.LogError($"Can't make report for pair {pair.Cur1}-{pair.Cur2}");
            }
            else
            {
                _log.LogInformation($"Successfull make report for pair {pair.Cur1}-{pair.Cur2}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
