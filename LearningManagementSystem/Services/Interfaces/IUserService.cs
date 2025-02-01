using LearningManagementSystem.Dto;

namespace LearningManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserDto userDto, string role); // Register a new user
        Task<bool> UpdateUserAsync(int userId, UpdateUserDto userDto); // Update an existing user
        Task<string> LoginAsync(LoginDto loginDto); // Login user and return JWT
        Task<bool> AssignRoleAsync(int userId, string role); // Assign a role to a user
        Task<List<GetUserDto>> GetUsersByRoleAsync(string role); // Get users by role
        Task<bool> RemoveRoleAsync(int userId, string role);

    }
}
