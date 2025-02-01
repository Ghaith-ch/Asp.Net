using System.Security.Claims;
using AutoMapper;
using E_commerce.Dto.UserDtos;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET /api/user/profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var userProfile = await _userService.GetUserProfileAsync(userId);
            if (userProfile == null)
                return NotFound();

            return Ok(userProfile);
        }

        // POST /api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserDto registerUserDto)
        {
            try
            {
                await _userService.RegisterUserAsync(registerUserDto);
                return Ok(new { message = "User registered successfully! Please check your email for verification." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/user/{userId}/verify-email?token=yourToken
        [HttpGet("{userId}/verify-email")]
        public async Task<IActionResult> VerifyEmail(int userId, [FromQuery] string token)
        {
            try
            {
                var isVerified = await _userService.VerifyEmailAsync(userId, token);
                if (!isVerified)
                    return BadRequest(new { message = "Email verification failed." });

                return Ok(new { message = "Email verified successfully!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
