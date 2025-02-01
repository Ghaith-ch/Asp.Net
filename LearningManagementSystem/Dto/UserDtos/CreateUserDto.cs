using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    // DTO for creating a user
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(256, ErrorMessage = "Username cannot exceed 256 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }
    }
}
