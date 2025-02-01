using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Job_Application_Management_System.Dto;
using Job_Application_Management_System.Models;
using Job_Application_Management_System.Repositories.Interfaces;
using Job_Application_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Job_Application_Management_System.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<int> CreateUserAsync(CreateUserDto userDto, string role)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);

            var createResult = await _userRepository.CreateUserAsync(user, userDto.Password);
            if (!createResult)
                throw new Exception("Failed to register user.");

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                throw new Exception($"Failed to assign role '{role}'.");

            return user.Id;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new UnauthorizedAccessException("Invalid username or password.");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var signingKey = _configuration["JWT:SigningKey"] ?? throw new InvalidOperationException("JWT SigningKey is missing.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!string.IsNullOrEmpty(userDto.FirstName))
                user.FirstName = userDto.FirstName;

            if (!string.IsNullOrEmpty(userDto.LastName))
                user.LastName = userDto.LastName;

            if (!string.IsNullOrEmpty(userDto.Address))
                user.Address = userDto.Address;

            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;

            // Handle password change
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                // Ensure current password is provided
                if (string.IsNullOrEmpty(userDto.CurrentPassword))
                {
                    throw new ArgumentException("Current password must be provided to change the password.");
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, userDto.CurrentPassword, userDto.Password);
                if (!changePasswordResult.Succeeded)
                {
                    var errors = string.Join(", ", changePasswordResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update the password: {errors}");
                }
            }

            return await _userRepository.UpdateUserAsync(user);
        }


        public async Task<bool> AssignRoleAsync(int userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);

            return true;
        }
    }
}
