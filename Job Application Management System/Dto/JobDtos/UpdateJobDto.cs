namespace Job_Application_Management_System.Dto
{
    public class UpdateJobDto
    {
        public  string? Title { get; set; }
        public  string? Description { get; set; }
        public  string? Location { get; set; }
        public  string? JobType { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
