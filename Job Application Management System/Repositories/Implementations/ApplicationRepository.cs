using Job_Application_Management_System.Data;
using Job_Application_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Job_Application_Management_System.Repositories.Interfaces;

namespace Job_Application_Management_System.Repositories.Implementations
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetApplicationByIdAsync(int applicationId)
        {
            // Ensure Job and Applicant details are always included to avoid NullReferenceException
            return await _context.Applications
                .Include(a => a.Job) // Include related job information
                .Include(a => a.Applicant) // Include applicant details
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<Application>> GetApplicationsForJobAsync(int jobId)
        {
            return await _context.Applications
                .Where(a => a.JobId == jobId)
                .Include(a => a.Applicant)
                .Include(a => a.Job)
                .ToListAsync();
        }

        public async Task<int> AddApplicationAsync(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return application.ApplicationId;
        }

        public async Task<bool> ApplicationExistsAsync(int jobId, int applicantId)
        {
            return await _context.Applications.AnyAsync(a => a.JobId == jobId && a.ApplicantId == applicantId);
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, string status)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null) return false;

            application.Status = status;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteApplicationAsync(int applicationId)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null) return false;

            _context.Applications.Remove(application);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsResumeFileUsedAsync(string resumeFilePath)
        {
            return await _context.Applications.AnyAsync(a => a.ResumeFilePath == resumeFilePath);
        }
    }
}
