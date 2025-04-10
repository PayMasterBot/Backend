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

        /// <summary>
        /// Добавить новую категорию для пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, он создается.
        /// Если категория не существует, она добавляется.
        /// Если категория для пользователя уже добавлена, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <param name="cat">Категория пользователя (должно быть заполнено поле Title)</param>
        /// <returns>Добавленная категория</returns>
        /// <response code="200">Категория для пользователя добавлена</response>
        /// <response code="400">Ошибка добавления</response>
        [Route("/api/category")]
        [HttpPost]
        public ActionResult<ExpenceCategory> AddCategory([FromQuery] int userId, [FromBody] ExpenceCategory cat)
        {
            var res = _rep.AddCategory(cat, new User { Id = userId });
            return res != null ? Ok(res) : BadRequest(res);
        }

        /// <summary>
        /// Получить список всех категорий пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует или у него нет категорий, вернется пустой список.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Список категорий пользователя</returns>
        /// <response code="200">Список категорий или пустой список</response>
        [Route("/api/category")]
        [HttpGet]
        public ActionResult<ICollection<ExpenceCategory>> AllCategories([FromQuery] int userId)
        {
            return Ok(_rep.GetCategories(userId));
        }

        /// <summary>
        /// Удалить категорию для пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// Если категория не существует, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <param name="id">ID категории</param>
        /// <returns>Успех (true) или неудача (false) удаления </returns>
        /// <response code="200">Категория удалена</response>
        /// <response code="400">Ошибка удаления</response>
        [Route("/api/category/{id}")]
        [HttpDelete]
        public ActionResult DeleteCategory([FromQuery] int userId, [FromRoute] int id)
        {
            if (_rep.DeleteCategory(id, userId))
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Получить категорию для пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// Если категория не существует, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <param name="id">ID категории</param>
        /// <returns>Категория</returns>
        /// <response code="200">Найденная категория</response>
        /// <response code="400">Ошибка получения категории</response>
        [Route("/api/category/{id}")]
        [HttpGet]
        public ActionResult<ExpenceCategory> GetCategory([FromQuery] int userId, [FromRoute] int id)
        {
            var res = _rep.GetCategory(id, userId);
            return res != null ? Ok(res) : BadRequest(res);
        }

        /// <summary>
        /// Добавить трату в категорию для пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// Если категория не существует, возвращается ошибка.
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <param name="id">ID категории</param>
        /// <param name="dto">Трата. Должны быть заполнены поля Date, Price, Title</param>
        /// <returns>Добавленная трата</returns>
        /// <response code="200">Трата для пользователя добавлена</response>
        /// <response code="400">Ошибка добавления траты</response>
        [Route("/api/category/{id}/spending")]
        [HttpPost]
        public ActionResult<Expence> AddSpending([FromQuery] int userId, [FromRoute] int id, [FromBody] ExpenceDto dto)
        {
            dto.CatId = id;
            dto.UserId = userId;
            var res = _rep.AddExpense(dto);
            return res != null ? Ok(res) : BadRequest(res);
        }

        /// <summary>
        /// Получить отчет о расходах пользователя
        /// </summary>
        /// <remarks>
        /// Если пользователь не существует, возвращается ошибка.
        /// 
        /// Отчет имеет вид:
        ///     {
        ///         "cur_month": [
        ///             "cat1": 100,
        ///             "cat2": 200
        ///         ],
        ///         "prev_month": [
        ///             "cat1": 150,
        ///             "cat2": 250
        ///         ]
        ///     }
        /// </remarks>
        /// <param name="userId">ID пользователя</param>
        /// <returns>JSON-отчет</returns>
        /// <response code="200">Отчет</response>
        /// <response code="400">Ошибка создания отчета</response>
        [Route("/api/category/report")]
        [HttpGet]
        public ActionResult<JsonObject> GetReport([FromQuery] int userId)
        {
            var res = _rep.GetReport(userId);
            return res != null ? Ok(res) : BadRequest(res);
        }
    }
}
