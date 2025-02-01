using AutoMapper;
using LearningManagementSystem.Dto;
using LearningManagementSystem.Models;

namespace LearningManagementSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping ApplicationUser to GetUserDto
            CreateMap<ApplicationUser, GetUserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()) // Roles handled separately.
                 .ForMember(dest => dest.Enrollments, opt => opt.MapFrom(src => src.Enrollments));


            // Mapping Course to GetCourseDto
            CreateMap<Course, GetCourseDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.UserName))
                .ForMember(dest => dest.Enrollments, opt => opt.MapFrom(src => src.Enrollments));

            // Mapping Enrollment to GetEnrollmentDto
            CreateMap<Enrollment, GetEnrollmentDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.UserName : "Unknown")) // Handle null Student
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : "Unknown"));  

            // Mapping CreateCourseDto to Course
            CreateMap<CreateCourseDto, Course>();

            // Mapping UpdateCourseDto to Course (for PATCH/PUT operations)
            CreateMap<UpdateCourseDto, Course>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 

            // Mapping CreateEnrollmentDto to Enrollment
            CreateMap<CreateEnrollmentDto, Enrollment>();

            // Mapping UpdateEnrollmentDto to Enrollment
            CreateMap<UpdateEnrollmentDto, Enrollment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 
        }
    }
}
