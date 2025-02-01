using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job_Application_Management_System.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Job description is required.")]
        [StringLength(1000, ErrorMessage = "Job description cannot exceed 1000 characters.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(250, ErrorMessage = "Location cannot exceed 250 characters.")]
        public required string Location { get; set; }

        [Required(ErrorMessage = "Job type is required.")]
        [StringLength(50, ErrorMessage = "Job type cannot exceed 50 characters.")]
        public required string JobType { get; set; }

        [Required(ErrorMessage = "Salary range is required.")]
        public decimal Salary { get; set; }

        [Required]
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int RecruiterId { get; set; }

        [ForeignKey("RecruiterId")]
        public ApplicationUser? Recruiter { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
