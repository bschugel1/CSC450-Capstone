using AutoMapper;
using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseApp.Controllers
{
    public class FeaturedController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public FeaturedController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _context.FeaturedCourses.Include(x => x.Course).ToList();
            return View(_mapper.Map<ICollection<FeaturedCourseVM>>(model));
        }
        [HttpGet]
        public IActionResult AddFeatured()
        {
            var model = _context.Courses
            .Where(x => !_context.FeaturedCourses
                .Select(y => y.CourseId)
                     .Contains(x.Id)
             );
            return View(_mapper.Map<ICollection<CourseVM>>(model));
        }
        [HttpPost]
        public IActionResult AddFeatured(long id)
        {
            var course = _context.Courses.FirstOrDefault(x => x.Id == id);
            var entity = new FeaturedCourseModel
            {
                CourseId = id,
                Name = course.Name,
                Feature = "",
                ExpirationDate = DateTime.Today.AddDays(30)
            };
            _context.Add(entity);
            _context.SaveChanges();
            return RedirectToActionPermanent("Index");
        }
    }
}
