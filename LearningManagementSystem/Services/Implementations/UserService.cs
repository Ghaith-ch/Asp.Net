using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LearningManagementSystem.Dto;
using LearningManagementSystem.Models;
using LearningManagementSystem.Repositories.Interfaces;
using LearningManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LearningManagementSystem.Services.Implementations
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
            var user = new ApplicationUser
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                Address = userDto.Address
            };

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
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
            if (user == null) throw new KeyNotFoundException("User not found.");

            if (!string.IsNullOrEmpty(userDto.Username))
                user.UserName = userDto.Username;

            if (!string.IsNullOrEmpty(userDto.Address))
                user.Address = userDto.Address;

            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;

            if (!string.IsNullOrEmpty(userDto.Password))
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userDto.Password);

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> AssignRoleAsync(int userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            return roleResult.Succeeded;
        }

        public async Task<List<GetUserDto>> GetUsersByRoleAsync(string role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            var userDtos = users.Select(user => _mapper.Map<GetUserDto>(user)).ToList();
            return userDtos;
        }


        public async Task<bool> RemoveRoleAsync(int userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var roleResult = await _userManager.RemoveFromRoleAsync(user, role);
            return roleResult.Succeeded;
        }

    }
}
