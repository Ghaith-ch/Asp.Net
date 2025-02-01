using Job_Application_Management_System.Data;
using Job_Application_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Job_Application_Management_System.Repositories.Interfaces;

namespace Job_Application_Management_System.Repositories.Implementations
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetJobByIdAsync(int jobId)
        {
            // Retrieve a job by ID, including its applications for context
            return await _context.Jobs.Include(j => j.Applications)
                                      .FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            // Retrieve all jobs, including recruiter information
            return await _context.Jobs
                .Include(j => j.Recruiter) // Include recruiter details
                .ToListAsync();
        }

        public async Task<bool> AddJobAsync(Job job)
        {
            // Add a new job to the database and save changes
            _context.Jobs.Add(job);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateJobAsync(Job job)
        {
            // Find the existing job and update it if found
            var existingJob = await _context.Jobs.FindAsync(job.JobId);
            if (existingJob == null) return false;

            _context.Entry(existingJob).CurrentValues.SetValues(job); // Update only changed values
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            // Find and delete the job if it exists
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null) return false;

            _context.Jobs.Remove(job);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
