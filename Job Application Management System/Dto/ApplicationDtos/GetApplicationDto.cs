using Job_Application_Management_System.Models;

namespace Job_Application_Management_System.Dto
{
    public class GetApplicationDto
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public int ApplicantId { get; set; }
        public string? CoverLetter { get; set; }
        public string? ResumeFilePath { get; set; } // Made nullable
        public string Status { get; set; } = "Pending";
        public DateTime ApplicationDate { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
    }
}
