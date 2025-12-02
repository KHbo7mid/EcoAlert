using EcoAlert.DTOs;
using EcoAlert.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace EcoAlert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", registerDto.Email);

                var response = await _authService.RegisterAsync(registerDto);

                return Ok(new
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = response.Token,
                    User = response.User,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for email: {Email}", registerDto.Email);
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

                var response = await _authService.LoginAsync(loginDto);

                return Ok(new
                {
                    Success = true,
                    Message = "Login successful",
                    Token = response.Token,
                    User = response.User,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for email: {Email}", loginDto.Email);
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var user = await _authService.GetUserProfileAsync(userId);

                return Ok(new
                {
                    Success = true,
                    User = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Not authenticated"
                });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // For JWT, logout is handled client-side by removing the token
            return Ok(new
            {
                Success = true,
                Message = "Logged out successfully"
            });
        }

        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Authenticated",
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Email = User.FindFirst(ClaimTypes.Email)?.Value,
                    Name = User.FindFirst(ClaimTypes.Name)?.Value,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value
                });
            }

            return Unauthorized(new
            {
                Success = false,
                Message = "Not authenticated"
            });
        }
    }
}
