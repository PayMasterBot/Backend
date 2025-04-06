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

        public CategoryController(PgPayContext ctx) : base()
        {
            _rep = new PgCategoryRepository(ctx);
        }

        [Route("/api/category")]
        public ActionResult<ExpenceCategory> AddCategory([FromQuery] int userId, [FromBody] ExpenceCategory cat)
        {
            var res = _rep.AddCategory(cat, new User { Id = userId });
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/category")]
        public ActionResult<ICollection<ExpenceCategory>> AllCategories([FromQuery] int userId)
        {
            return Ok(_rep.GetCategories(userId));
        }

        [Route("/api/category/{id}")]
        public ActionResult DeleteCategory([FromQuery] int userId, [FromRoute] int id)
        {
            if (_rep.DeleteCategory(id, userId))
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("/api/category/{id}")]
        public ActionResult<ExpenceCategory> GetCategory([FromQuery] int userId, [FromRoute] int id)
        {
            var res = _rep.GetCategory(id, userId);
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/category/{id}/spending")]
        public ActionResult<Expence> AddSpending([FromQuery] int userId, [FromRoute] int id, [FromBody] ExpenceDto dto)
        {
            dto.CatId = id;
            dto.UserId = userId;
            var res = _rep.AddExpense(dto);
            return res != null ? Ok(res) : BadRequest(res);
        }

        [Route("/api/category/report")]
        public ActionResult<JsonObject> GetReport([FromQuery] int userId)
        {
            var res = _rep.GetReport(userId);
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
