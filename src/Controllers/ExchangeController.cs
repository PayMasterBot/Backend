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

        /// <summary>
        /// Получить баланс пользователя на Binance
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// Если пользователь не авторизован на Binance, возвращается ошибка.
        /// Формат баланса: ???
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <returns>JSON-баланс</returns>
        /// <response code="200">Баланс пользователя</response>
        /// <response code="400">Ошибка получения баланса</response>
        [Route("/api/exchange/balance")]
        [HttpGet]
        public ActionResult<JsonObject> GetBalance([FromQuery] int userId)
        {
            var res = _rep.GetBalance(userId);
            return res != null ? Ok(res) : BadRequest(res);
        }

        /// <summary>
        /// Аутентифицировать пользователя на Binance
        /// </summary>
        /// <remarks>
        /// Если токен не валиден, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <param name="token">Токен Binance</param>
        /// <returns>Успех аутентификации</returns>
        /// <response code="200">Успех</response>
        /// <response code="400">Ошибка</response>
        [Route("/api/exchange/auth")]
        [HttpPost]
        public ActionResult MakeAuth([FromQuery] int userId, [FromBody] string token)
        {
            return _rep.ExchangeAuth(userId, token) ? Ok() : BadRequest();
        }
    }
}
