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

        public CurrencyController(PgPayContext ctx) : base()
        {
            _rep = new PgCurrencyRepository(ctx);
        }

        [Route("/api/currency-pair/rate")]
        public ActionResult<double> GetRate([FromQuery] string Cur1, [FromQuery] string Cur2)
        {
            try
            {
                return Ok(_rep.GetExchangeRate(Cur1, Cur2));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Route("/api/currency-pair")]
        public ActionResult<ICollection<ExchangeRateSubscription>> AllCurrencyPairs([FromQuery] int userId)
        {
            return Ok(_rep.GetCurrencyPairs(userId));
        }

        [Route("/api/currency-pair")]
        public ActionResult DeleteCurrencyPair([FromQuery] int userId, [FromBody] CurrencyPairDto pair)
        {
            ExchangeRateSubscription sub = pair;
            sub.UserId = userId;
            return _rep.DeleteCurrencyPairSubscription(sub) ? Ok() : BadRequest();
        }

        [Route("/api/currency-pair")]
        public ActionResult<ExchangeRateSubscription> AddCurrencyPair([FromQuery] int userId, [FromBody] CurrencyPairDto pair)
        {
            ExchangeRateSubscription sub = pair;
            sub.UserId = userId;
            var res = _rep.AddCurrencyPairSubscription(sub);
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/currency-pair/report")]
        public ActionResult<ExchangeRateSubscription> GetReport([FromBody] CurrencyPairDto pair)
        {
            var res = _rep.GetReport(pair);
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
