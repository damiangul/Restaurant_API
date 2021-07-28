using Microsoft.AspNetCore.Mvc;
using Restaurant_API.Models;
using Restaurant_API.Services;

namespace Restaurant_API.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }
        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            var dishId = _service.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{dishId}", null);
        }
    }
}