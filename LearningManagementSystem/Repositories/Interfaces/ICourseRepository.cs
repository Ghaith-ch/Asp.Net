using LearningManagementSystem.Models;

namespace LearningManagementSystem.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetCourseByIdAsync(int courseId); 
        Task<IEnumerable<Course>> GetAllCoursesAsync(); 
        Task<bool> CreateCourseAsync(Course course); 
        Task<bool> UpdateCourseAsync(Course course); 
        Task<bool> DeleteCourseAsync(int courseId); 
        Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(int instructorId);

    }
}
