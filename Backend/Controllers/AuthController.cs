using AuthenticationModule.DTO.Request;
using AuthenticationModule.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationModule.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO req)
        {
            var result = await _authService.RefreshTokenAsync(req.RefreshToken);
            return Ok(result);
        }

    }
}
