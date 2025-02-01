using AutoMapper;
using LearningManagementSystem.Dto;
using LearningManagementSystem.Repositories.Interfaces;
using LearningManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IMapper _mapper;

        public EnrollmentController(IEnrollmentRepository enrollmentRepository, IEnrollmentService enrollmentService, IMapper mapper)
        {
            _enrollmentRepository = enrollmentRepository;
            _enrollmentService = enrollmentService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult> CreateEnrollment([FromForm] CreateEnrollmentDto enrollmentDto)
        {
            var result = await _enrollmentService.CreateEnrollmentAsync(enrollmentDto);
            if (!result) return BadRequest("Failed to create enrollment.");
            return Ok(new { Message = "Enrollment created successfully." });
        }

        [HttpGet("{enrollmentId}")]
        [Authorize]
        public async Task<ActionResult<GetEnrollmentDto>> GetEnrollmentById(int enrollmentId)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null) return NotFound("Enrollment not found.");
            var enrollmentDto = _mapper.Map<GetEnrollmentDto>(enrollment);
            return Ok(enrollmentDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetEnrollmentDto>>> GetAllEnrollments()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            var enrollmentDtos = _mapper.Map<IEnumerable<GetEnrollmentDto>>(enrollments);
            return Ok(enrollmentDtos);
        }

        [HttpPut("{enrollmentId}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult> UpdateEnrollment(int enrollmentId, [FromForm] UpdateEnrollmentDto enrollmentDto)
        {
            try
            {
                var result = await _enrollmentService.UpdateEnrollmentAsync(enrollmentId, enrollmentDto);
                if (!result) return BadRequest("Failed to update enrollment.");
                return Ok(new { Message = "Enrollment updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{enrollmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEnrollment(int enrollmentId)
        {
            var result = await _enrollmentRepository.DeleteEnrollmentAsync(enrollmentId);
            if (!result) return NotFound("Enrollment not found.");
            return Ok(new { Message = "Enrollment deleted successfully." });
        }
    }
}
