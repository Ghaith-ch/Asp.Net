using Job_Application_Management_System.Dto;

namespace Job_Application_Management_System.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<int> AddApplicationAsync(CreateApplicationDto createApplicationDto, int applicantId);

        Task<bool> UpdateApplicationStatusAsync(int applicationId, string status);

        Task<bool> DeleteApplicationAsync(int applicationId);
    }
}
