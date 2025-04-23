
using api.Constants;
using api.Dtos.Auth;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route(RouteConstants.AUTH)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                var newUser = await _authServices.RegisterAsync(registerDto);
                // Console.WriteLine(JsonConvert.SerializeObject(newUser, Formatting.Indented));
                return Ok(newUser);

            }
            catch (Exception e)
            {

                return StatusCode(500, "Error register: " + e);
            }
        }
    }
}
