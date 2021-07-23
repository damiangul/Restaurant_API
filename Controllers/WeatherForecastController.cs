using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IWeatherForcastService _service;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForcastService service)
        {
            _logger = logger;
            _service = service;
        }
    /*
                                                                PONIZEJ ZNAJDUJA SIE ROZNE IMPLEMENTACJE METODY GET
                                                                Mamy jak zmieniać routy albo skąd można przyjmować zmienne.
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var result = _service.Get();

            return result;
        }



        //Różne sposoby na te same czasowniki HTTP
        [HttpGet("anotherLink")]
        public IEnumerable<WeatherForecast> Get2()
        {
            var result = _service.Get();

            return result;
        }

        //Przekazanie parametrów
        //From query to cos takiego: currentDay/30?take=50
        [HttpGet]
        [Route("currentDay/{max}")]
        public IEnumerable<WeatherForecast> Get3([FromQuery] int take, [FromRoute] int max)
        {
            var result = _service.Get();

            return result;
        }
    */

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Get([FromQuery]int results, [FromBody] TemperateRequest temperateRequest)
        {
            if (results < 0 || temperateRequest.minTemperature > temperateRequest.maxTemperature)
                return BadRequest();

            var result = _service.Get(results, temperateRequest.minTemperature, temperateRequest.maxTemperature);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<string> Hello([FromBody]string name)
        {
            //HttpContext.Response.StatusCode = 401;

            //return StatusCode(401, $"Hello {name}");

            return NotFound($"Hello {name}");
        }
    }
}
