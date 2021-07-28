using System.Collections.Generic;
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

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDto dish = _service.GetById(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            var result = _service.GetAll(restaurantId);

            return Ok(result);
        }

        [HttpDelete("{dishId}")]
        public ActionResult DeleteByID([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _service.RemoveDishById(restaurantId, dishId);

            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int restaurantId)
        {
            _service.RemoveAll(restaurantId);

            return NoContent();
        }
    }
}