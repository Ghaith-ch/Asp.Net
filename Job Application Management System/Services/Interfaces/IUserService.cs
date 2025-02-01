using Job_Application_Management_System.Dto;

namespace Job_Application_Management_System.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserDto userDto, string role);  
        Task<string> LoginAsync(LoginDto loginDto); 
        Task<bool> UpdateUserAsync(int userId, UpdateUserDto userDto); 
        Task<bool> AssignRoleAsync(int userId, string role); // New role assignment logic
    }
}
