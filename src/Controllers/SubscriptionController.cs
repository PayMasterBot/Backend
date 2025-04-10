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

        [Route("/api/subscription")]
        [HttpPost]
        public ActionResult<Subscription> AddSubscription([FromQuery] int userId, [FromBody] Subscription sub)
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

        [Route("/api/subscription")]
        [HttpGet]
        public ActionResult<ICollection<Subscription>> AllSubscriptions([FromQuery] int userId)
        {
            var res = _rep.GetSubscriptions(userId);
            _log.LogInformation($"Successfull get all categories for user {userId}. Total {res.Count} categories.");
            return Ok(res);
        }

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
