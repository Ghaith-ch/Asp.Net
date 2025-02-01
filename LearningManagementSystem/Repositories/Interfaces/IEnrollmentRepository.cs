using LearningManagementSystem.Models;

namespace LearningManagementSystem.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetEnrollmentByIdAsync(int enrollmentId);
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task<bool> CreateEnrollmentAsync(Enrollment enrollment);
        Task<bool> UpdateEnrollmentAsync(Enrollment enrollment);
        Task<bool> DeleteEnrollmentAsync(int enrollmentId);
    }
}
