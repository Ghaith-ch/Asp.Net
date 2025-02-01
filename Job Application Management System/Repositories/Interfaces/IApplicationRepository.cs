using Job_Application_Management_System.Models;

namespace Job_Application_Management_System.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application?> GetApplicationByIdAsync(int applicationId); // Fetch a specific application by its unique ID

        Task<IEnumerable<Application>> GetApplicationsForJobAsync(int jobId); // Retrieve all applications for a given job ID

        Task<int> AddApplicationAsync(Application application); // Add a new application to the database and return its ID

        Task<bool> ApplicationExistsAsync(int jobId, int applicantId); // Check if an applicant has already applied for a specific job

        Task<bool> UpdateApplicationStatusAsync(int applicationId, string status); // Update the status of an application (e.g., "Pending", "Accepted", "Rejected")

        Task<bool> DeleteApplicationAsync(int applicationId); // Remove an application by its ID
        
        Task<bool> IsResumeFileUsedAsync(string resumeFilePath); // Check if a resume file is used by other applications
    }
}
