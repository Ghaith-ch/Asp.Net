using LearningManagementSystem.Data;
using LearningManagementSystem.Models;
using LearningManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(int instructorId)
        {
            return await _context.Courses
                                .Where(c => c.InstructorId == instructorId)
                                .Include(c => c.Enrollments) 
                                .Include(c => c.Instructor)  
                                .ToListAsync();
        }


        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses.Include(c => c.Instructor)
                                         .Include(c => c.Enrollments)
                                         .ThenInclude(e => e.Student)
                                         .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.Include(c => c.Instructor)
                                         .Include(c => c.Enrollments)
                                         .ToListAsync();
        }

        public async Task<bool> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return false;

            _context.Courses.Remove(course);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
