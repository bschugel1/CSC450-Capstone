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
    public class AuthorController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public AuthorController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public IActionResult Index()
        {

            var model = _context.Courses.Where(x => x.AuthorId == long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return View(_mapper.Map<ICollection<CourseVM>>(model));
        }

        [HttpPost]
        public IActionResult Create(CourseCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var entity = new CourseModel {

                    Id = 0,
                    Name = model.Name,
                    CourseCode = model.CourseCode,
                    Subject = model.Subject,
                    Description = model.Description,
                    AuthorId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                                                       
                };
                _context.Add(entity);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CourseCreateVM());
        }

        [HttpGet]
        public IActionResult Error(string id)
        {
            return View("Error", id);
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            var model = _context.Courses.FirstOrDefault(x => x.Id == id);

            if (model == default)
            {
                return RedirectToAction(nameof(Error), new
                {
                    id = "The requested course was not found!"

                });
            }
            else
            {
                return View(_mapper.Map<CourseEditVM>(model));
            }            
        }
        
        [HttpPost]
        public IActionResult Edit(CourseEditVM model)
        {
            if (ModelState.IsValid)
            {
                var entity = _context.Courses.FirstOrDefault(x => x.Id == model.Id);

                entity.Name = model.Name;
                entity.CourseCode = model.CourseCode;
                entity.Subject = model.Subject;
                entity.Description = model.Description;

              
                _context.Update(entity);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

    }
}
