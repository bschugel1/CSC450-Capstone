using CourseAppCloud.DAL;
using CourseAppCloud.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CourseAppCloud.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseContext _context;


        public CourseController(CourseContext context)
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
