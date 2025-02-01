using E_commerce.Dto.UserDtos;
namespace E_commerce.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetUserProfileAsync(string userId);
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<bool> VerifyEmailAsync(int userId, string token);
    }
}
