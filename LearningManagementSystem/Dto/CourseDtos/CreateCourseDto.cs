using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(200, ErrorMessage = "Course title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Course description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Instructor ID is required.")]
        public int InstructorId { get; set; }
    }
}
