using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Application.Services;
using src.Application.Utils;
using src.Presentation.RequestModels;
using System.Security.Claims;

namespace src.Presentation.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var (status,token) = await _service.Register(request.Email,request.Username,request.Password);

            if(token is null)
            {
                return BadRequest(ResultHelper.GetDescription(status));
            }

            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequests request)
        {
            var (status, token) = await _service.Login(request.Email, request.Password);

            if(token is null)
            {
                return BadRequest(ResultHelper.GetDescription(status));
            }

            return Ok(new { Token = token });
        }

        [HttpGet("role")]
        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult GetRole()
        {
            var role = User.FindFirst(ClaimTypes.Role)!.Value;

            return Ok(new { Role = role });
        }

    }
}
