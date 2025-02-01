using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    public class UpdateCourseDto
    {
        [StringLength(200, ErrorMessage = "Course title cannot exceed 200 characters.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Course description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        public int? InstructorId { get; set; }
    }
}
