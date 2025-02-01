using LearningManagementSystem.Data;
using LearningManagementSystem.Models;
using LearningManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Repositories.Implementations
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int enrollmentId)
        {
            return await _context.Enrollments.Include(e => e.Student)
                                             .Include(e => e.Course)
                                             .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments.Include(e => e.Student)
                                             .Include(e => e.Course)
                                             .ToListAsync();
        }

        public async Task<bool> CreateEnrollmentAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateEnrollmentAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null) return false;

            _context.Enrollments.Remove(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
