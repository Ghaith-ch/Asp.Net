using LearningManagementSystem.Dto;
using LearningManagementSystem.Models;
using LearningManagementSystem.Repositories.Interfaces;
using LearningManagementSystem.Services.Interfaces;

namespace LearningManagementSystem.Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<bool> CreateEnrollmentAsync(CreateEnrollmentDto enrollmentDto)
        {
            var enrollment = new Enrollment
            {
                StudentId = enrollmentDto.StudentId,
                CourseId = enrollmentDto.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                Grade = enrollmentDto.Grade 
            };

            return await _enrollmentRepository.CreateEnrollmentAsync(enrollment);
        }


        public async Task<bool> UpdateEnrollmentAsync(int enrollmentId, UpdateEnrollmentDto enrollmentDto)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null)
                throw new KeyNotFoundException("Enrollment not found.");

            if (enrollmentDto.Grade.HasValue)
                enrollment.Grade = enrollmentDto.Grade;

            return await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);
        }
    }
}
