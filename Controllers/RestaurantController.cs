using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant_API.Models;
using Restaurant_API.Services;

namespace Restaurant_API.Controllers
{
    [Route("api/restaurant")]
    //Ten atrybut pozwala automatycznie na walidacje modelu. Nie potrzeba już w metodach robić sprawdzania poprawności wpisanych danych.
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] RestaurantUpdateDto restaurantUpdateDto)
        {
            //[ApiController] załatwia sprawę
            // if(!ModelState.IsValid)
            //     return BadRequest(ModelState);

            _restaurantService.UpdateRestaurant(id, restaurantUpdateDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            //[ApiController] załatwia sprawę
            // if(!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            int Id = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{Id}", null);
        }
        
        [HttpGet]
        [Authorize(Policy = "HasNationality")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            
            var restaurantsDtos = _restaurantService.GetAll();

            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        //Akcja zezwala na zapytania bez nagłówka autoryzacji
        [AllowAnonymous]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurantDto = _restaurantService.GetById(id);

            return Ok(restaurantDto);
        }
    }
}