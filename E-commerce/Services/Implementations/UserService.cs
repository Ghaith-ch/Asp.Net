using AutoMapper;
using E_commerce.Dto.UserDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace E_commerce.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? null : _mapper.Map<UserProfileDto>(user);
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists.");

            var user = _mapper.Map<ApplicationUser>(registerUserDto);
            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user.");

            string role = string.IsNullOrEmpty(registerUserDto.Role) ? "Customer" : registerUserDto.Role;
            var roleResult = await _userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
                throw new InvalidOperationException("Failed to assign role to user.");

            // Generate email verification token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Send verification email
            await SendVerificationEmailAsync(user.Email, user.Id, token);

            return true;
        }

        public async Task<bool> VerifyEmailAsync(int userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        private async Task SendVerificationEmailAsync(string email, int userId, string token)
        {
            var verificationUrl = $"http://localhost:5219/api/user/{userId}/verify-email?token={Uri.EscapeDataString(token)}";
            var emailBody = $"Please verify your email by clicking the following link: <a href='{verificationUrl}'>Verify Email</a>";
            
            await _emailService.SendEmailAsync(email, "Verify Your Email", emailBody);
        }
    }
}
