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
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Preview(long id)
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
                return View("preview", _mapper.Map<CoursePreviewVM>(model));
            }
        }
        [HttpGet]
        public IActionResult Course(long id, long? parentid)
        {
            var model = _context.Courses.Include(x => x.Sections).FirstOrDefault(x => x.Id == id);
            if (model == default)
            {
                return RedirectToAction(nameof(Error), new
                {
                    id = "The requested course was not found!"
                });
            }
            else
            {
                ViewData["ParentSectionId"] = parentid ?? 0;
                return View(_mapper.Map<CourseVM>(model));
            }
        }

        [HttpGet]
        public IActionResult MyCourses(long id)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = _context.UserCourses.Where(x => x.User.Id == userId);
            return View(_mapper.Map<ICollection<CourseVM>>(model.Select(x => x.Course)));
        }

        [HttpPost]
        public IActionResult Register(long id)
        {
            var entity = new UserCourseModel
            {
                UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CourseId = id
            };

            _context.Add(entity);
            _context.SaveChanges();
            return RedirectToAction(nameof(MyCourses));
        }
        [HttpGet]
        public IActionResult Error(string id)
        {
            return View("Error", id);
        }
    }
}
