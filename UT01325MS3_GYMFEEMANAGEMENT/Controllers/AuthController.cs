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
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest(new { success = false, message = "NIC and Password are required." });
            }

            try
            {
                var (token, isAdmin) = await _authService.AuthenticateMemberAsync(loginDto);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, message = "Invalid NIC or Password" });
                }

                return Ok(new
                {
                    success = true,
                    token = token,
                    isAdmin = isAdmin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    detail = ex.Message
                });
            }
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterRequestDto registerDto)
        {
            try
            {
                await _authService.RegisterAdminAsync(registerDto);
                return Ok(new { success = true, message = "Admin registered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred.", detail = ex.Message });
            }
        }


    }
}
