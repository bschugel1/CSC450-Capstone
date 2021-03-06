﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.DAL;
using CourseApp.ViewModels;
using AutoMapper;

namespace CourseApp.ViewComponents
{
    public class CourseEditSectionsViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CourseEditSectionsViewComponent(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone, long courseId, long? selectedSection)
        {
            var items = await GetItemsAsync(maxPriority, isDone, courseId);
            items.SelectedSection = selectedSection ?? 0;
            return View(items);
        }
        private async Task<CourseEditVM> GetItemsAsync(int maxPriority, bool isDone, long courseId)
        {
            return _mapper.Map<CourseEditVM>(_context.Courses.FirstOrDefault(x => x.Id == courseId));
        }
        
    }
}