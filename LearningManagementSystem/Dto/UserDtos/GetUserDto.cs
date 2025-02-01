using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Dto
{
    public class GetUserDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Address { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        // New property to include enrollments
        public List<GetEnrollmentDto> Enrollments { get; set; } = new List<GetEnrollmentDto>();
    }

}
