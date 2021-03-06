﻿using AutoMapper;
using CourseApp.Models;
using CourseApp.ViewModels;
using System.Linq;

namespace CourseApp.DAL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, RegistrationVM>()
                .ForMember(u => u.Email, opt => opt.MapFrom(x => x.UserName)).ReverseMap();
            CreateMap<UserModel, UserVM>()
                .ForMember(d => d.Courses, o => o.MapFrom(s => s.UserCourses.Select(x => x.User)));
            CreateMap<CourseModel, CourseVM>()
              .ForMember(d => d.Users, o => o.MapFrom(s => s.UserCourses.Select(x => x.Course)));
            CreateMap<CourseModel, CourseCreateVM>().ReverseMap();
            CreateMap<CourseModel, CourseEditVM>().ReverseMap();
            CreateMap<CourseModel, CoursePreviewVM>().ReverseMap();
            CreateMap<CourseModel, CourseVM>().ReverseMap();
            CreateMap<FeaturedCourseModel, FeaturedCourseVM>().ReverseMap();
            CreateMap<UserModel, AccountVM>().ReverseMap();
            CreateMap<UserModel, AccountEditVM>().ReverseMap();
            CreateMap<SectionModel, SectionCreateVM>().ReverseMap();
            CreateMap<SectionModel, FileUploadVM>().ReverseMap();
            CreateMap<EmbedModel, EmbedVideoVM>().ReverseMap();
            CreateMap<TransactionModel, TransactionVM>().ReverseMap();
        }

    }
}
