using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    public class UpdateEnrollmentDto
    {
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public int? Grade { get; set; }
    }
}
