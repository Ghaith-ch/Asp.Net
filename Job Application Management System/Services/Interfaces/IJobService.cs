using Job_Application_Management_System.Models;

namespace Job_Application_Management_System.Services.Interfaces
{
    public interface IJobService
    {
        Task<bool> AddJobAsync(Job job); // Method to add a new job with necessary properties (e.g., posted date)

        Task<bool> UpdateJobAsync(Job job); // Method to update an existing job with business rule validation (e.g., expiry date)
    }
}
