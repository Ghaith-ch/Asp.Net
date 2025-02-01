using LearningManagementSystem.Models;

namespace LearningManagementSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(int userId); // Fetch user by ID
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(); // Fetch all users
        Task<bool> CreateUserAsync(ApplicationUser user, string password); // Register a new user
        Task<bool> UpdateUserAsync(ApplicationUser user); // Update an existing user
        Task<bool> DeleteUserAsync(int userId); // Delete a user
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string role); // Fetch users by role
        Task<List<string>> GetUserRolesAsync(ApplicationUser user); // Fetch roles for a user

    }
}
