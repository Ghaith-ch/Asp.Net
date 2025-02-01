using LearningManagementSystem.Dto;

namespace LearningManagementSystem.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<bool> CreateEnrollmentAsync(CreateEnrollmentDto enrollmentDto);
        Task<bool> UpdateEnrollmentAsync(int enrollmentId, UpdateEnrollmentDto enrollmentDto);
    }
}
