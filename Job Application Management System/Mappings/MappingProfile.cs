using AutoMapper;
using Job_Application_Management_System.Dto;
using Job_Application_Management_System.Models;

namespace Job_Application_Management_System.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Job mappings
            CreateMap<Job, GetJobDto>()
                .ForMember(dest => dest.Applications, opt => opt.Ignore());

            CreateMap<CreateJobDto, Job>();

            CreateMap<UpdateJobDto, Job>()
                .ForMember(dest => dest.PostedDate, opt => opt.Ignore()) // Preserve original PostedDate
                .ForMember(dest => dest.RecruiterId, opt => opt.Ignore()) // RecruiterId should not be updated
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null && (
                        opt.DestinationMember.Name != "Salary" || (decimal?)srcMember != 0
                    )
                ));

            // User mappings
            CreateMap<ApplicationUser, GetUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be handled separately

            CreateMap<CreateUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password hash is handled by UserManager

            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // Application mappings
            CreateMap<Application, GetApplicationDto>()
                .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => $"{src.Applicant.FirstName} {src.Applicant.LastName}"))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
                .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => src.ApplicationDate));

            CreateMap<CreateApplicationDto, Application>()
                .ForMember(dest => dest.ApplicationDate, opt => opt.Ignore()) // Set automatically in the model
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Default status is set in the model
                .ForMember(dest => dest.ResumeFilePath, opt => opt.Ignore()); // ResumeFilePath is assigned in service layer

            CreateMap<UpdateApplicationStatusDto, Application>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
