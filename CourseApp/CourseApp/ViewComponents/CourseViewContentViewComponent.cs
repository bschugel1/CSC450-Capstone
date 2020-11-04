using AutoMapper;
using CourseApp.DAL;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class CourseViewContentViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CourseViewContentViewComponent(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        int maxPriority, bool isDone, long courseId)
        {
            var items = await GetItemsAsync(maxPriority, isDone, courseId);
            return View(items);
        }
        private async Task<CourseVM> GetItemsAsync(int maxPriority, bool isDone, long courseId)
        {

            return _mapper.Map<CourseVM>(_context.Courses.FirstOrDefault(x => x.Id == courseId));
        }
    }
}
