using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(200, ErrorMessage = "Course title cannot exceed 200 characters.")]
        public required string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Course description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        // Foreign key for the instructor teaching this course
        [Required(ErrorMessage = "Instructor ID is required.")]
        public int InstructorId { get; set; }

        // Navigation property to link the course to the instructor
        [ForeignKey("InstructorId")]
        public required ApplicationUser Instructor { get; set; } // Removed 'required'

        // Navigation property for enrollments (Many-to-Many relationship with ApplicationUser)
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
