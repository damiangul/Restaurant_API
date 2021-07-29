using Microsoft.AspNetCore.Mvc;
using Restaurant_API.Models;
using Restaurant_API.Services;

namespace Restaurant_API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _service.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _service.GenerateJwt(dto);
            return Ok(token);
        }
    }
}