namespace LearningManagementSystem.Dto
{
    public class GetEnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int? Grade { get; set; }
    }
}
