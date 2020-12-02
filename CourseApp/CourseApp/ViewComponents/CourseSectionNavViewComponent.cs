using AutoMapper;
using CourseApp.DAL;
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
    public class CourseSectionNavViewComponent : ViewComponent
    {

        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CourseSectionNavViewComponent(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(long courseId)
        {
            var entity = _context.Courses.Include(x => x.Sections).FirstOrDefault(x => x.Id == courseId);
            var model = new CourseEditVM
            {
                Id = entity.Id,
                Sections = entity.Sections.ToList()               
            };

            return View(model);
        }
    }
}
