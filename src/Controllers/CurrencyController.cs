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

        /// <summary>
        /// Получить курс валютной пары
        /// </summary>
        /// <remarks>
        /// Если одна из валют пары не существует, возвращается ошибка.
        /// </remarks>
        /// <param name="Cur1">Тэг первой валюты пары</param>
        /// <param name="Cur2">Тэг второй валюты пары</param>
        /// <returns>Курс пары</returns>
        /// <response code="200">Курс обмена</response>
        /// <response code="400">Ошибка получения курса обмена</response>
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

        /// <summary>
        /// Получить список всех подписок на валютные пары пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается пустой список.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <returns>JSON-баланс</returns>
        /// <response code="200">Список подписок пользователя</response>
        [Route("/api/currency-pair")]
        [HttpGet]
        public ActionResult<ICollection<ExchangeRateSubscription>> AllCurrencyPairs([FromQuery] int userId)
        {
            _log.LogInformation($"Successfull get list of subscribed pairs for user {userId}");
            return Ok(_rep.GetCurrencyPairs(userId));
        }

        /// <summary>
        /// Удалить подписку на валютную пару для пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// Если пользователь не подписан на валютную пару, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Успех удаления</returns>
        /// <response code="200">Подписка удалена</response>
        /// <response code="400">Ошибка удаления подписки</response>
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

        /// <summary>
        /// Добавить подписку на валютную пару для пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// Если пользователь уже подписан на валютную пару, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <param name="pair">Валютная пара (порядок валют важен)</param>
        /// <returns>Подписка на валютную пару</returns>
        /// <response code="200">Подписка добавлена</response>
        /// <response code="400">Ошибка добавления подписки</response>
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

        /// <summary>
        /// Получить отчет о валютной паре
        /// </summary>
        /// <remarks>
        /// Если одна из валют не существует возвращается ошибка.
        /// 
        /// Отчет имеет вид: [[...], [...]]
        /// Каждая свеча содержит Open time (timestamp), Open, High, Low, Close, Volume, Close time (timestamp),
        /// Quote asset volume, Number of trades, Taker buy base asset volume, Taker buy quote asset volume, Ignore (0).
        /// </remarks>
        /// <param name="pair">Валютная пара (порядок валют важен)</param>
        /// <returns>JSON-отчет</returns>
        /// <response code="200">Отчет</response>
        /// <response code="400">Ошибка создания отчета</response>
        [Route("/api/currency-pair/report")]
        [HttpPost]
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
