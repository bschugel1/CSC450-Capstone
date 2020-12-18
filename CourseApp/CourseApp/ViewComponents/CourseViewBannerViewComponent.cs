using AutoMapper;
using CourseApp.DAL;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class CourseViewBannerViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CourseViewBannerViewComponent(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(long courseId)
        {
            var items = await GetItemsAsync(courseId);
            return View(items);
        }
        private async Task<CourseVM> GetItemsAsync(long courseId)
        {
            return _mapper.Map<CourseVM>(_context.Courses.FirstOrDefault(x => x.Id == courseId));
        }
    }
}
