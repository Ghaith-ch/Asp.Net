using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(256, ErrorMessage = "Username cannot exceed 256 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
