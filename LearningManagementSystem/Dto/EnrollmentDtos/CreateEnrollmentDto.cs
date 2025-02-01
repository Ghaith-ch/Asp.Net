using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    public class CreateEnrollmentDto
    {
        [Required(ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }

        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public int? Grade { get; set; }
    }
}
