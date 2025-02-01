using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        // Foreign key for the student who is enrolled
        [Required(ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }

        // Navigation property to link the enrollment to the student
        [ForeignKey("StudentId")]
        public  ApplicationUser? Student { get; set; } 

        // Foreign key for the course the student is enrolled in
        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }

        // Navigation property to link the enrollment to the course
        [ForeignKey("CourseId")]
        public Course? Course { get; set; } 

        // The date the student enrolled in the course
        [Required(ErrorMessage = "Enrollment date is required.")]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        // Optional grade for the course, nullable in case it has not been assigned yet
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public int? Grade { get; set; } // Nullable because the grade might not be assigned initially
    }
}
