using System.ComponentModel.DataAnnotations;

namespace Job_Application_Management_System.Dto
{
    public class UpdateUserDto
    {
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [StringLength(100, ErrorMessage = "Current password cannot exceed 100 characters.")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; } // Add this property
    }
}
