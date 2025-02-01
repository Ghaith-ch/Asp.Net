using Job_Application_Management_System.Models;

namespace Job_Application_Management_System.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(int userId); // Fetch a specific user by their unique ID

        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(); // Retrieve all registered users

        Task<bool> CreateUserAsync(ApplicationUser user, string password); // Create a new user with the provided password

        Task<bool> UpdateUserAsync(ApplicationUser user); // Update user information (e.g., email, name, address)

        Task<bool> DeleteUserAsync(int userId); // Remove a user by their ID

        Task<List<string>> GetUserRolesAsync(ApplicationUser user); // Retrieve all roles assigned to a specific user
    }

}
