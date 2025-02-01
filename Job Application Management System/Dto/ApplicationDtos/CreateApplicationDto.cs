using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Job_Application_Management_System.Dto
{
    public class CreateApplicationDto
    {
        [Required]
        public int JobId { get; set; }

        [StringLength(1000, ErrorMessage = "Cover letter cannot exceed 1000 characters.")]
        public string? CoverLetter { get; set; }

        [Required(ErrorMessage = "Resume is required.")]
        public required IFormFile  Resume { get; set; } 
    }
}
