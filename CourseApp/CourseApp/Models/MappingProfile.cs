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

            CreateMap<UserModel, AccountVM>()
                .ForMember(u => u.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(u => u.LastName, opt => opt.MapFrom(x => x.LastName))
                .ReverseMap();
        }

    }
}
