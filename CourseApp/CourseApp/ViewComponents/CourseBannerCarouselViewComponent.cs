using AutoMapper;
using CourseApp.DAL;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class CourseBannerCarouselViewComponent : ViewComponent
    {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;

            public CourseBannerCarouselViewComponent(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IViewComponentResult> InvokeAsync(string featured)
            {
                var items = await GetItemsAsync(featured);
                return View(items);
            }

            private async Task<ICollection<FeaturedCourseVM>> GetItemsAsync(string featured)
            {
                return _mapper.Map<ICollection<FeaturedCourseVM>>(_context.FeaturedCourses.Where(x => x.Feature == featured).ToList());

            }
        }
    }

