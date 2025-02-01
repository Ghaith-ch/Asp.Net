using E_commerce.Dto.UserDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromForm] LoginUserDto loginUserDto)
        {
            try
            {
                var token = await _authService.LoginAsync(loginUserDto);
                return Ok(new { message = "Login successful!", token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromForm] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "User not found.");
                return NotFound(ModelState);
            }

            var token = await _authService.GeneratePasswordResetTokenAsync(user);

            // Send user ID and token in the email
            await _emailService.SendEmailAsync(email, "Reset Your Password",
                $"Your user ID is: {user.Id}<br>Your reset token is: {token}<br>" +
                $"Use these to reset your password through the API.");

            return Ok(new { message = "Password reset instructions sent to your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var isReset = await _authService.ResetPasswordAsync(resetPasswordDto);
                if (!isReset)
                    return BadRequest(new { message = "Failed to reset password." });

                return Ok(new { message = "Password reset successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
