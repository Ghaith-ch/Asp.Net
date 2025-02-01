using System.Security.Claims;
using AutoMapper;
using Job_Application_Management_System.Dto;
using Job_Application_Management_System.Models;
using Job_Application_Management_System.Repositories.Interfaces;
using Job_Application_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Job_Application_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobController(IJobRepository jobRepository, IUserRepository userRepository, IJobService jobService, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _userRepository = userRepository;
            _jobService = jobService;
            _mapper = mapper;
        }

        // GET: api/Job/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetJobDto>> GetJobById(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound(new { Message = "Job not found." });
            }

            var jobDto = _mapper.Map<GetJobDto>(job);
            return Ok(jobDto);
        }

        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetJobDto>>> GetAllJobs()
        {
            var jobs = await _jobRepository.GetAllJobsAsync();
            var jobDtos = _mapper.Map<IEnumerable<GetJobDto>>(jobs);
            return Ok(jobDtos);
        }

        // POST: api/Job
        [HttpPost]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult> AddJob([FromForm] CreateJobDto createJobDto)
        {
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (User.IsInRole("Admin"))
            {
                // Admins can specify a RecruiterId or default to themselves
                if (createJobDto.RecruiterId == null || createJobDto.RecruiterId == 0)
                {
                    createJobDto.RecruiterId = loggedInUserId; // Default to Admin's ID
                }
                else
                {
                    var recruiter = await _userRepository.GetUserByIdAsync(createJobDto.RecruiterId.Value);
                    if (recruiter == null || !(await _userRepository.GetUserRolesAsync(recruiter)).Contains("Recruiter"))
                    {
                        return BadRequest(new { Message = "Invalid RecruiterId." });
                    }
                }
            }
            else if (User.IsInRole("Recruiter"))
            {
                // Recruiters can only assign themselves as the RecruiterId
                createJobDto.RecruiterId = loggedInUserId;
            }
            else
            {
                return Forbid(); // Only Admins and Recruiters can add jobs
            }

            // Map the DTO to the model
            var job = _mapper.Map<Job>(createJobDto);

            var result = await _jobService.AddJobAsync(job);
            if (!result)
            {
                return BadRequest(new { Message = "Failed to add the job." });
            }

            return CreatedAtAction(nameof(GetJobById), new { id = job.JobId }, job);
        }

        // PUT: api/Job/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult> UpdateJob(int id, [FromForm] UpdateJobDto updateJobDto, [FromQuery] int? newRecruiterId = null)
        {
            var existingJob = await _jobRepository.GetJobByIdAsync(id);
            if (existingJob == null)
            {
                return NotFound(new { Message = "Job not found." });
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (User.IsInRole("Recruiter"))
            {
                // Restrict Recruiters to update only their own jobs
                if (existingJob.RecruiterId != loggedInUserId)
                {
                    return Forbid(); // Recruiters cannot update jobs they do not own
                }

                // Restrict Recruiters from modifying the RecruiterId
                if (newRecruiterId.HasValue)
                {
                    return BadRequest(new { Message = "Recruiters cannot modify the RecruiterId." });
                }
            }

            if (User.IsInRole("Admin") && newRecruiterId.HasValue)
            {
                // Admins can update the RecruiterId
                var recruiter = await _userRepository.GetUserByIdAsync(newRecruiterId.Value);
                if (recruiter == null || !(await _userRepository.GetUserRolesAsync(recruiter)).Contains("Recruiter"))
                {
                    return BadRequest(new { Message = "Invalid RecruiterId." });
                }

                existingJob.RecruiterId = newRecruiterId.Value;
            }

            // Retain the old ExpiryDate if the new one is not provided
            if (!updateJobDto.ExpiryDate.HasValue)
            {
                updateJobDto.ExpiryDate = existingJob.ExpiryDate;
            }

            _mapper.Map(updateJobDto, existingJob);

            var result = await _jobService.UpdateJobAsync(existingJob);
            if (!result)
            {
                return BadRequest(new { Message = "Failed to update the job." });
            }

            return Ok(new { Message = "Job updated successfully." });
        }

        // DELETE: api/Job/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult> DeleteJob(int id)
        {
            var existingJob = await _jobRepository.GetJobByIdAsync(id);
            if (existingJob == null)
            {
                return NotFound(new { Message = "Job not found." });
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Restrict Recruiters to delete only their own jobs
            if (User.IsInRole("Recruiter") && existingJob.RecruiterId != loggedInUserId)
            {
                return Forbid(); // Recruiters cannot delete jobs they do not own
            }

            var result = await _jobRepository.DeleteJobAsync(id);
            if (!result)
            {
                return BadRequest(new { Message = "Failed to delete the job." });
            }

            return Ok(new { Message = "Job deleted successfully." });
        }
    }
}
