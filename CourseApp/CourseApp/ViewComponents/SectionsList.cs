using CourseApp.DAL;
using CourseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class SectionsList : ViewComponent
    {
        private ApplicationContext _context;

        public SectionsList(ApplicationContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int maxPriority, bool isDone)
        {
            var sections = new List<SectionModel>
            {


            };
            return View(sections);
        }




           
        }
    }

