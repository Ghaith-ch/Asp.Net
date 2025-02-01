using LearningManagementSystem.Dto;
using LearningManagementSystem.Models;
using LearningManagementSystem.Repositories.Interfaces;
using LearningManagementSystem.Services.Interfaces;

namespace LearningManagementSystem.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;

        public CourseService(ICourseRepository courseRepository, IUserRepository userRepository)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> CreateCourseAsync(CreateCourseDto courseDto)
        {
            // Fetch the instructor by ID to set the Instructor property
            var instructor = await _userRepository.GetUserByIdAsync(courseDto.InstructorId);
            if (instructor == null)
                throw new KeyNotFoundException("Instructor not found.");

            // Validate if the user has the "Instructor" role
            var roles = await _userRepository.GetUserRolesAsync(instructor);
            if (!roles.Contains("Instructor"))
                throw new InvalidOperationException("User is not an instructor.");

            // Create the Course object
            var course = new Course
            {
                Title = courseDto.Title,
                Description = courseDto.Description,
                InstructorId = courseDto.InstructorId,
                Instructor = instructor
            };

            return await _courseRepository.CreateCourseAsync(course);
        }

        public async Task<bool> UpdateCourseAsync(int courseId, UpdateCourseDto courseDto)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            if (!string.IsNullOrEmpty(courseDto.Title))
                course.Title = courseDto.Title;

            if (!string.IsNullOrEmpty(courseDto.Description))
                course.Description = courseDto.Description;

            if (courseDto.InstructorId.HasValue)
            {
                // Fetch the instructor by ID
                var instructor = await _userRepository.GetUserByIdAsync(courseDto.InstructorId.Value);
                if (instructor == null)
                    throw new KeyNotFoundException("Instructor not found.");

                // Validate if the user has the "Instructor" role
                var roles = await _userRepository.GetUserRolesAsync(instructor);
                if (!roles.Contains("Instructor"))
                    throw new InvalidOperationException("User is not an instructor.");

                course.InstructorId = courseDto.InstructorId.Value;
                course.Instructor = instructor;
            }

            return await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task<IEnumerable<Course>> GetTaughtCoursesByInstructorAsync(int instructorId)
        {
            // Validate if the user exists and is an instructor
            var instructor = await _userRepository.GetUserByIdAsync(instructorId);
            if (instructor == null)
                throw new KeyNotFoundException("Instructor not found.");

            var roles = await _userRepository.GetUserRolesAsync(instructor);
            if (!roles.Contains("Instructor"))
                throw new InvalidOperationException("User is not an instructor.");

            // Fetch courses for the instructor
            return await _courseRepository.GetCoursesByInstructorIdAsync(instructorId);
        }
    }
}
