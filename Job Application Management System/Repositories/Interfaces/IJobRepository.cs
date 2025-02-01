using Job_Application_Management_System.Models;

namespace Job_Application_Management_System.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetJobByIdAsync(int jobId); // Fetch a specific job by its unique ID

        Task<IEnumerable<Job>> GetAllJobsAsync(); // Retrieve all available jobs

        Task<bool> AddJobAsync(Job job); // Add a new job to the database

        Task<bool> UpdateJobAsync(Job job); // Update the details of an existing job

        Task<bool> DeleteJobAsync(int jobId); // Remove a job by its ID
    }

}
