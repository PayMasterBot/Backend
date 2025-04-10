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
                return Ok(_rep.GetExchangeRate(Cur1, Cur2));
            }
            catch
            {
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
            return _rep.DeleteCurrencyPairSubscription(sub) ? Ok() : BadRequest();
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
            return res != null ? Ok(res) : BadRequest(res);
        }

        /// <summary>
        /// Получить отчет о валютной паре
        /// </summary>
        /// <remarks>
        /// Если одна из валют не существует возвращается ошибка.
        /// 
        /// Отчет имеет вид: ???
        /// </remarks>
        /// <param name="pair">Валютная пара (порядок валют важен)</param>
        /// <returns>JSON-отчет</returns>
        /// <response code="200">Отчет</response>
        /// <response code="400">Ошибка создания отчета</response>
        [Route("/api/currency-pair/report")]
        [HttpGet]
        public ActionResult<ExchangeRateSubscription> GetReport([FromBody] CurrencyPairDto pair)
        {
            var res = _rep.GetReport(pair);
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
