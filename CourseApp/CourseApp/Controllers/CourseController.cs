using CourseApp.DAL;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace CourseApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationContext _context;


        public CourseController(ApplicationContext context)
        {
            _context = context;

         
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(CourseCreateVM model)
        {
            return View(model);
        }

    }
}
