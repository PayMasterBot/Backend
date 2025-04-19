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
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionRepository _rep;
        private ILogger<SubscriptionController> _log;

        public SubscriptionController(PgPayContext ctx, ILogger<SubscriptionController> logger) : base()
        {
            _rep = new PgSubscriptionRepository(ctx);
            _log = logger;
        }

        /// <summary>
        /// Добавить новую подписку для пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sub">Подписка пользователя (должны быть заполнены поле Title, Price)</param>
        /// <returns>Добавленная подписка</returns>
        /// <response code="200">Подписка для пользователя добавлена</response>
        /// <response code="400">Ошибка добавления</response>
        [Route("/api/subscription")]
        [HttpPost]
        public ActionResult<Subscription> AddSubscription([FromQuery] int userId, [FromBody] SubscriptionDto sub)
        {
            sub.UserId = userId;
            var res = _rep.AddSubscription(sub);
            if (res is null)
            {
                _log.LogError($"Can't add subscription with title {sub.Title} for user {userId}");
            }
            else
            {
                _log.LogInformation($"Successfull add subscription with title {sub.Title} for user {userId}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }

        /// <summary>
        /// Получить все подписки пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается пустой список.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Список всех подписок</returns>
        /// <response code="200">Список всех подписок пользователя</response>
        [Route("/api/subscription")]
        [HttpGet]
        public ActionResult<ICollection<Subscription>> AllSubscriptions([FromQuery] int userId)
        {
            var res = _rep.GetSubscriptions(userId);
            _log.LogInformation($"Successfull get all categories for user {userId}. Total {res.Count} categories.");
            return Ok(res);
        }

        /// <summary>
        /// Удалить подписку для пользователя
        /// </summary>
        /// <remarks>
        /// Если подписка не существует, возвращается ошибка.
        /// </remarks>
        /// <param name="id">ID подписки</param>
        /// <returns>Успех или неудача удаления подписки</returns>
        /// <response code="200">Подписка успешно удалена</response>
        /// <response code="400">Ошибка удаления</response>
        [Route("/api/subscription/{id}")]
        [HttpDelete]
        public ActionResult DeleteSubscription([FromRoute] int id)
        {
            if (_rep.DeleteSubscription(id))
            {
                _log.LogInformation($"Successfull delete subscription with Id {id}");
                return Ok();
            }
            _log.LogInformation($"Can't delete subscription with Id {id}");
            return BadRequest();
        }

        /// <summary>
        /// Получить подписку для пользователя
        /// </summary>
        /// <remarks>
        /// Если подписка не существует, возвращается ошибка.
        /// </remarks>
        /// <param name="id">ID подписки</param>
        /// <returns>Подписка</returns>
        /// <response code="200">Подписка для пользователя</response>
        /// <response code="400">Ошибка получения подписки</response>
        [Route("/api/subscription/{id}")]
        [HttpGet]
        public ActionResult<Subscription> GetSubscription([FromRoute] int id)
        {
            var res = _rep.GetSubscription(id);
            if (res is null)
            {
                _log.LogError($"Can't get subscription with id {id}");
            }
            else
            {
                _log.LogInformation($"Successfull get subscription with id {id}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
