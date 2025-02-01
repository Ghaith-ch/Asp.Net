using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    // DTO for updating user details
    public class UpdateUserDto
    {

         [StringLength(256, ErrorMessage = "Username cannot exceed 256 characters.")]
        public string? Username { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public string? Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string? Password { get; set; }
    }
}
