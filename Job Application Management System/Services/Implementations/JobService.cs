using Job_Application_Management_System.Models;
using Job_Application_Management_System.Repositories.Interfaces;
using Job_Application_Management_System.Services.Interfaces;

namespace Job_Application_Management_System.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<bool> AddJobAsync(Job job)
        {
            // Set the posted date to the current date and time in UTC before saving the job
            job.PostedDate = DateTime.UtcNow;

            // Save the job to the database using the repository
            return await _jobRepository.AddJobAsync(job);
        }

        public async Task<bool> UpdateJobAsync(Job job)
        {
            // Validate that the expiry date is not in the past
            if (job.ExpiryDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Expiry date cannot be in the past.");
            }

            // Update the job in the database using the repository
            return await _jobRepository.UpdateJobAsync(job);
        }
    }
}
