namespace Job_Application_Management_System.Dto
{
    public class CreateJobDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }
        public required string JobType { get; set; }
        public decimal Salary { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int? RecruiterId { get; set; }
    }
}
