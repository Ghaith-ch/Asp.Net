using System.ComponentModel.DataAnnotations;

namespace Job_Application_Management_System.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}
