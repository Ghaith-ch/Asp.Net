namespace LearningManagementSystem.Dto
{
    public class GetCourseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int InstructorId { get; set; }
        public string? InstructorName { get; set; } 
        public List<GetEnrollmentDto> Enrollments { get; set; } = new List<GetEnrollmentDto>();

    }
}
