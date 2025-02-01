using System.Security.Claims;
using AutoMapper;
using Job_Application_Management_System.Dto;
using Job_Application_Management_System.Repositories.Interfaces;
using Job_Application_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Job_Application_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationService _applicationService;
        private readonly IJobRepository _jobRepository;
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public ApplicationController(
            IApplicationRepository applicationRepository,
            IApplicationService applicationService,
            IJobRepository jobRepository,
            IJobService jobService,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _applicationService = applicationService;
            _jobRepository = jobRepository;
            _jobService = jobService;
            _mapper = mapper;
        }

        // GET: api/Application/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetApplicationDto>> GetApplicationById(int id)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound(new { Message = "Application not found." });
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (User.IsInRole("Admin") || 
                (User.IsInRole("Recruiter") && application.Job.RecruiterId == loggedInUserId) || 
                application.ApplicantId == loggedInUserId)
            {
                var applicationDto = _mapper.Map<GetApplicationDto>(application);
                return Ok(applicationDto);
            }

            return Forbid();
        }

        // GET: api/Application/Job/{jobId}
        [HttpGet("Job/{jobId}")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<ActionResult<IEnumerable<GetApplicationDto>>> GetApplicationsForJob(int jobId)
        {
            if (!User.IsInRole("Admin"))
            {
                var job = await _jobRepository.GetJobByIdAsync(jobId);
                var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (job == null || job.RecruiterId != loggedInUserId)
                {
                    return Forbid();
                }
            }

            var applications = await _applicationRepository.GetApplicationsForJobAsync(jobId);
            var applicationDtos = _mapper.Map<IEnumerable<GetApplicationDto>>(applications);
            return Ok(applicationDtos);
        }

        // POST: api/Application
        [HttpPost]
        [Authorize(Roles = "Applicant")]
        public async Task<ActionResult> ApplyForJob([FromForm] CreateApplicationDto createApplicationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var applicantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var applicationId = await _applicationService.AddApplicationAsync(createApplicationDto, applicantId);

                return CreatedAtAction(nameof(GetApplicationById), new { id = applicationId }, new { Message = "Application submitted successfully.", ApplicationId = applicationId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // PUT: api/Application/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<ActionResult> UpdateApplicationStatus(int id, [FromForm] UpdateApplicationStatusDto updateStatusDto)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound(new { Message = "Application not found." });
            }

            if (!User.IsInRole("Admin"))
            {
                var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (application.Job == null || application.Job.RecruiterId != loggedInUserId)
                {
                    return Forbid();
                }
            }

            var result = await _applicationService.UpdateApplicationStatusAsync(id, updateStatusDto.Status);
            if (!result)
            {
                return NotFound(new { Message = "Application not found or could not be updated." });
            }

            return Ok(new { Message = "Application status updated successfully." });
        }

        // DELETE: api/Application/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Applicant")]
        public async Task<ActionResult> DeleteApplication(int id)
        {
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            var application = await _applicationRepository.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound(new { Message = "Application not found." });
            }

            if (isAdmin || application.ApplicantId == loggedInUserId)
            {
                var result = await _applicationService.DeleteApplicationAsync(id);
                if (!result)
                {
                    return BadRequest(new { Message = "Failed to delete the application." });
                }

                return Ok(new { Message = "Application deleted successfully." });
            }

            return Forbid();
        }
    }
}
