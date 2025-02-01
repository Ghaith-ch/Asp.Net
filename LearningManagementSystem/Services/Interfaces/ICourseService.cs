using LearningManagementSystem.Dto;
using LearningManagementSystem.Models;

namespace LearningManagementSystem.Services.Interfaces
{
    public interface ICourseService
    {
        Task<bool> CreateCourseAsync(CreateCourseDto courseDto);
        Task<bool> UpdateCourseAsync(int courseId, UpdateCourseDto courseDto);
        Task<IEnumerable<Course>> GetTaughtCoursesByInstructorAsync(int instructorId); // New method
    }
}
