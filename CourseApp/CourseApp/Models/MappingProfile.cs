using AutoMapper;
using CourseApp.ViewModels;

namespace CourseApp.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationVM, UserModel>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<CourseModel, CourseCreateVM>().ReverseMap();
            CreateMap<CourseModel, CourseEditVM>().ReverseMap();
            CreateMap<CourseModel, CourseVM>().ReverseMap();

        }

    }
}
