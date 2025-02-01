using LearningManagementSystem.Dto;
using AutoMapper;
using LearningManagementSystem.Repositories.Interfaces;
using LearningManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository courseRepository, ICourseService courseService, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _courseService = courseService;
            _mapper = mapper;
        }

        // GET: api/Course/instructor/{instructorId}/courses
        [HttpGet("instructor/{instructorId}/courses")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<IEnumerable<GetCourseDto>>> GetTaughtCourses(int instructorId)
        {
            try
            {
                var courses = await _courseService.GetTaughtCoursesByInstructorAsync(instructorId);
                if (!courses.Any())
                {
                    return NotFound("No courses found for this instructor.");
                }

                var courseDtos = _mapper.Map<IEnumerable<GetCourseDto>>(courses);
                return Ok(courseDtos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // GET: api/Course/{courseId}
        [HttpGet("{courseId}")]
        [Authorize]
        public async Task<ActionResult<GetCourseDto>> GetCourseById(int courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null) return NotFound("Course not found.");

            var courseDto = _mapper.Map<GetCourseDto>(course);

            return Ok(courseDto);
        }

        // GET: api/Course
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetCourseDto>>> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();

            var courseDtos = _mapper.Map<IEnumerable<GetCourseDto>>(courses);

            return Ok(courseDtos);
        }

        // POST: api/Course
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateCourse([FromForm] CreateCourseDto courseDto)
        {
            var result = await _courseService.CreateCourseAsync(courseDto);
            if (!result) return BadRequest("Failed to create course.");
            return Ok(new { Message = "Course created successfully." });
        }

        // PUT: api/Course/{courseId}
        [HttpPut("{courseId}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult> UpdateCourse(int courseId, [FromForm] UpdateCourseDto courseDto)
        {
            try
            {
                var result = await _courseService.UpdateCourseAsync(courseId, courseDto);
                if (!result) return BadRequest("Failed to update course.");
                return Ok(new { Message = "Course updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Course/{courseId}
        [HttpDelete("{courseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCourse(int courseId)
        {
            var result = await _courseRepository.DeleteCourseAsync(courseId);
            if (!result) return NotFound("Course not found.");
            return Ok(new { Message = "Course deleted successfully." });
        }
    }
}
