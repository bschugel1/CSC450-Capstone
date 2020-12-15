using AutoMapper;
using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class CourseRegisteredUsersViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CourseRegisteredUsersViewComponent(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(long courseId)
        { 
            var entity = _mapper.Map<CourseEditVM>(_context.Courses.FirstOrDefault(x => x.Id == courseId));
            var userCourses = _context.UserCourses.Where(x => x.CourseId == entity.Id).Include(x => x.User);
            var model = new CourseEditVM
            {
                Id = entity.Id,
                Users = userCourses.ToList()
            };

            return View(model);
        }
    }
}
