using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blecommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
            var response = await _authService.Register(
            new User
            {
                Email = request.Email,

            }, request.Password);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login (UserLoginDto model)
        {
            var response = await _authService.Login(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }
    }
}
