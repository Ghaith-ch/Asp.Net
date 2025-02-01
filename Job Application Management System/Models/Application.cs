using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job_Application_Management_System.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public required Job Job { get; set; }

        [Required]
        public int ApplicantId { get; set; }

        [ForeignKey("ApplicantId")]
        public required ApplicationUser Applicant { get; set; }

        [Required(ErrorMessage = "Application date is required.")]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        [StringLength(1000, ErrorMessage = "Cover letter cannot exceed 1000 characters.")]
        public string? CoverLetter { get; set; }

        public string? ResumeFilePath { get; set; } // Made nullable to align with service

        [Required(ErrorMessage = "Application status is required.")]
        [StringLength(50, ErrorMessage = "Application status cannot exceed 50 characters.")]
        public string Status { get; set; } = "Pending";
    }
}
