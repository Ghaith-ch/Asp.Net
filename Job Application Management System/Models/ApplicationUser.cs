using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Job_Application_Management_System.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public required string LastName { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        public ICollection<Job> PostedJobs { get; set; } = new List<Job>();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
