using Microsoft.AspNetCore.Mvc;
using UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests;
using UT01325MS3_GYMFEEMANAGEMENT.Services;

namespace UT01325MS3_GYMFEEMANAGEMENT.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            // Validate input
            if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { success = false, message = "NIC and Password are required." });
            }

            var token = await _authService.AuthenticateMemberAsync(loginDto);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { success = false, message = "Invalid NIC or Password" });
            }

            return Ok(new { success = true, token = token });
        }

    }
}
