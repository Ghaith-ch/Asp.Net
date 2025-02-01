using System.Collections.Generic;

namespace Job_Application_Management_System.Dto
{
    public class GetUserDto
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<GetJobDto>? PostedJobs { get; set; } = new();
        public List<GetApplicationDto>? Applications { get; set; } = new List<GetApplicationDto>(); // Optional
    }
}
