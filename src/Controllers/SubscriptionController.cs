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

        public SubscriptionController(PgPayContext ctx) : base()
        {
            _rep = new PgSubscriptionRepository(ctx);
        }

        [Route("/api/subscription")]
        public ActionResult<Subscription> AddSubscription([FromQuery] int userId, [FromBody] Subscription sub)
        {
            sub.UserId = userId;
            var res = _rep.AddSubscription(sub);
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/subscription")]
        public ActionResult<ICollection<Subscription>> AllSubscriptions([FromQuery] int userId)
        {
            return Ok(_rep.GetSubscriptions(userId));
        }

        [Route("/api/subscription/{id}")]
        public ActionResult DeleteSubscription([FromRoute] int id)
        {
            if (_rep.DeleteSubscription(id))
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("/api/subscription/{id}")]
        public ActionResult<Subscription> GetSubscription([FromRoute] int id)
        {
            var res = _rep.GetSubscription(id);
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
