using AutoMapper;
using CourseApp.ViewModels;
using System.Linq;

namespace CourseApp.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, RegistrationVM>()
                .ForMember(u => u.Email, opt => opt.MapFrom(x => x.UserName));
            CreateMap<UserModel, UserVM>()
                .ForMember(d => d.Courses, o => o.MapFrom(s => s.UserCourses.Select(x => x.User)));
            CreateMap<CourseModel, CourseVM>()
              .ForMember(d => d.Users, o => o.MapFrom(s => s.UserCourses.Select(x => x.Course)));

            CreateMap<CourseModel, CourseCreateVM>().ReverseMap();
            CreateMap<CourseModel, CourseEditVM>().ReverseMap();
            CreateMap<CourseModel, CoursePreviewVM>().ReverseMap();
            CreateMap<CourseModel, CourseVM>().ReverseMap();

        }

    }
}
