using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;

namespace CourseApp.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CourseController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _context.Courses.ToList();
            return View(_mapper.Map<ICollection<CourseVM>>(model));
        }

        [AllowAnonymous]
        public IActionResult Request(long id)
        {
            var model = _context.Courses.Find(id);

            if (model == default)
            {
                return RedirectToAction(nameof(Error), new
                {
                    id = "The requested course was not found!"

                });
            }
            else
            {
                return View("course", _mapper.Map<CourseVM>(model));
            }

        }
      
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(string id)
        {
            return View("Error", id);
        }       
    }
}
