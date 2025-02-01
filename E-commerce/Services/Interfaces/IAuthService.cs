using E_commerce.Dto.UserDtos;
using E_commerce.Models;

namespace E_commerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginUserDto loginUserDto);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
