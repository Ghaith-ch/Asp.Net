using Job_Application_Management_System.Dto;
using Job_Application_Management_System.Models;
using Job_Application_Management_System.Repositories.Interfaces;
using Job_Application_Management_System.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;

namespace Job_Application_Management_System.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment; // Used for accessing the server's root directory for file storage

        public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper, IWebHostEnvironment environment)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<int> AddApplicationAsync(CreateApplicationDto createApplicationDto, int applicantId)
        {
            if (await _applicationRepository.ApplicationExistsAsync(createApplicationDto.JobId, applicantId))
            {
                throw new InvalidOperationException("You have already applied for this job.");
            }

            ValidateResumeFile(createApplicationDto.Resume);
            string resumePath = await SaveResumeFile(createApplicationDto.Resume);

            var application = _mapper.Map<Application>(createApplicationDto);
            application.ApplicantId = applicantId;
            application.ResumeFilePath = resumePath;

            return await _applicationRepository.AddApplicationAsync(application);
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, string status)
        {
            return await _applicationRepository.UpdateApplicationStatusAsync(applicationId, status);
        }

        public async Task<bool> DeleteApplicationAsync(int applicationId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null) return false;

            // Only check if the resume is used elsewhere if ResumeFilePath is not null or empty
            bool isResumeUsedElsewhere = string.IsNullOrEmpty(application.ResumeFilePath) 
                || await _applicationRepository.IsResumeFileUsedAsync(application.ResumeFilePath);

            if (!isResumeUsedElsewhere && !string.IsNullOrEmpty(application.ResumeFilePath))
            {
                var filePath = Path.Combine(_environment.WebRootPath, application.ResumeFilePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            return await _applicationRepository.DeleteApplicationAsync(applicationId);
        }


        // Utility: Save the resume file to the server and return the relative file path
        private async Task<string> SaveResumeFile(IFormFile resume)
        {
            var uploadDirectory = Path.Combine(_environment.WebRootPath, "resumes");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory); // Ensure the directory exists
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(resume.FileName);
            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await resume.CopyToAsync(stream);
            }

            return Path.Combine("resumes", uniqueFileName); // Return relative path
        }

        // Utility: Validate the uploaded resume file for allowed types and size
        private void ValidateResumeFile(IFormFile resume)
        {
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" }; // Restrict allowed file types
            var maxFileSize = 5 * 1024 * 1024; // Set maximum allowed file size to 5MB

            var fileExtension = Path.GetExtension(resume.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Only .pdf, .doc, or .docx files are allowed.");
            }

            if (resume.Length > maxFileSize)
            {
                throw new InvalidOperationException("File size exceeds the 5MB limit.");
            }
        }
    }
}
