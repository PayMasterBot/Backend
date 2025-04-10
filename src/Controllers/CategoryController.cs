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
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository _rep;
        private ILogger<CategoryController> _log;

        public CategoryController(PgPayContext ctx, ILogger<CategoryController> logger) : base()
        {
            _rep = new PgCategoryRepository(ctx);
            _log = logger;
        }

        [Route("/api/category")]
        [HttpPost]
        public ActionResult<ExpenceCategory> AddCategory([FromQuery] int userId, [FromBody] ExpenceCategory cat)
        {
            var res = _rep.AddCategory(cat, new User { Id = userId });
            if (res is null)
            {
                _log.LogError($"Can't add category with title {cat.Title} for user {userId}");
            }
            else
            {
                _log.LogInformation($"Successfull add category with title {cat.Title} for user {userId}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/category")]
        [HttpGet]
        public ActionResult<ICollection<ExpenceCategory>> AllCategories([FromQuery] int userId)
        {
            var res = _rep.GetCategories(userId);
            _log.LogInformation($"Successfull get all categories for user {userId}. Total {res.Count} categories.");
            return Ok(res);
        }

        [Route("/api/category/{id}")]
        [HttpDelete]
        public ActionResult DeleteCategory([FromQuery] int userId, [FromRoute] int id)
        {
            if (_rep.DeleteCategory(id, userId))
            {
                _log.LogInformation($"Successfull delete category with id {id} for user {userId}");
                return Ok();
            }
            _log.LogError($"Can't delete category with id {id} for user {userId}");
            return BadRequest();
        }

        [Route("/api/category/{id}")]
        [HttpGet]
        public ActionResult<ExpenceCategory> GetCategory([FromQuery] int userId, [FromRoute] int id)
        {
            var res = _rep.GetCategory(id, userId);
            if (res is null)
            {
                _log.LogError($"Can't get category with id {id} for user {userId}");
            }
            else
            {
                _log.LogInformation($"Successfull get category with id {id} for user {userId}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/category/{id}/spending")]
        [HttpPost]
        public ActionResult<Expence> AddSpending([FromQuery] int userId, [FromRoute] int id, [FromBody] ExpenceDto dto)
        {
            dto.CatId = id;
            dto.UserId = userId;
            var res = _rep.AddExpense(dto);
            if (res is null)
            {
                _log.LogError($"Can't add spending in category with id {id} for user {userId}");
            }
            else
            {
                _log.LogInformation($"Successfull add spending in category with id {id} for user {userId}. {dto.Title}: {dto.Price}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/category/report")]
        [HttpGet]
        public ActionResult<JsonObject> GetReport([FromQuery] int userId)
        {
            var res = _rep.GetReport(userId);
            if (res is null)
            {
                _log.LogError($"Can't make report for user {userId}");
            }
            else
            {
                _log.LogInformation($"Successfull make report for user {userId}");
            }
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
