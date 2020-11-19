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
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using CourseApp.Helpers;
using System;

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
                var entity = new CourseModel
                {
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
        public IActionResult Edit(long id, bool? showadd, bool? showupload, string mediatype, long? sectionid, long? parentid)
        {
            var model = _context.Courses.Include(s => s.Sections).ThenInclude(i => i.Items).ThenInclude(i => i.Section).FirstOrDefault(x => x.Id == id);
            if (!User.IsCurrentAuthor(model.AuthorId))
            {
                return RedirectToAction("Index", "Home");
            }

            if (model == default)
            {
                return RedirectToAction(nameof(Error), new
                {
                    id = "The requested course was not found!"
                });
            }
            else
            {
                ViewData["MediaType"] = mediatype;
                ViewData["ShowAddForm"] = showadd ?? false;
                ViewData["ShowUploadForm"] = showupload ?? false;
                ViewData["SectionId"] = sectionid;
                ViewData["ParentSectionId"] = parentid;

                return View(_mapper.Map<CourseEditVM>(model));
            }
        }

        [HttpPost]
        public IActionResult Edit(CourseEditVM model)
        {

            if (ModelState.IsValid)
            {
                var entity = _context.Courses.Include(x => x.Sections).FirstOrDefault(x => x.Id == model.Id);
                if (!User.IsCurrentAuthor(entity.AuthorId))
                {
                    ModelState.AddModelError("Form", "You cannot edit this course. You are not the Author!");
                    return View(model);
                }



                _context.Update(entity);
                _context.SaveChanges();

                return View(model);
            }
            else
            {
                return View(model);
            }
        }


        [HttpPost]
        public IActionResult MoveUp(long id, int displayOrder)
        {

            var model = _context.Sections.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
            if (ModelState.IsValid)
            {

                var sections = _context.Sections.Where(x => x.CourseId == model.CourseId && x.ParentSectionId == model.ParentSectionId);
                var entity = sections.FirstOrDefault(x => x.Id == model.Id);
                var course = _context.Courses.FirstOrDefault(x => x.Id == model.CourseId);
                var predicate = _context.Sections.Where(x => x.ParentSectionId == model.ParentSectionId && x.CourseId == model.CourseId);
                int size = predicate.Count();
                var prev = _context.Sections.FirstOrDefault(predicate => predicate.CourseId == model.CourseId && predicate.DisplayOrder == model.DisplayOrder - 1);

                if (entity != default && prev != default)
                {
                    if (prev.DisplayOrder < size && entity.DisplayOrder > 1)
                    {

                        // Move previous to current
                        prev.DisplayOrder = model.DisplayOrder;

                        // Move current entity up previous
                        entity.DisplayOrder = model.DisplayOrder - 1;



                        _context.Update(prev);
                        _context.Update(entity);
                        ReorderSections(sections.ToList());
                        _context.SaveChanges();
                    }
                    else
                    {
                        ReorderSections(sections.ToList());
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = model.CourseId });
            }
            return View();
        }

        [HttpPost]
        public IActionResult MoveDown(long id, int displayOrder)
        {
            var model = _context.Sections.FirstOrDefault(x => x.Id == id);
            if (ModelState.IsValid)
            {
                
                var sections = _context.Sections.Where(x => x.CourseId == model.CourseId && x.ParentSectionId == model.ParentSectionId);
                var entity = sections.FirstOrDefault(x => x.Id == model.Id);

                var course = _context.Courses.FirstOrDefault(x => x.Id == model.CourseId);
                var predicate = _context.Sections.Where(x => x.ParentSectionId == model.ParentSectionId && x.CourseId == model.CourseId);

                int size = predicate.Count();
                //get previous section
                var next = _context.Sections.FirstOrDefault(predicate => predicate.CourseId == model.CourseId && predicate.DisplayOrder == model.DisplayOrder + 1);

                if (entity != default && next != default)
                {
                    if (next.DisplayOrder > 0 && entity.DisplayOrder < size)
                    {
                        // Move previous to current
                        next.DisplayOrder = model.DisplayOrder;
                        // Move current entity up previous
                        entity.DisplayOrder = model.DisplayOrder + 1;
                        _context.Update(next);
                        _context.Update(entity);
                        ReorderSections(sections.ToList());
                        _context.SaveChanges();

                    }
                    else
                    {
                        ReorderSections(sections.ToList());
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = model.CourseId });
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddSection(SectionCreateVM model)
        {
            int size = _context.Sections.Where(x => x.CourseId == model.CourseId && x.ParentSectionId == model.ParentSectionId).Count();

            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {                    
                    var entity = _context.Sections.FirstOrDefault(x => x.Id == model.Id);
                    if (entity != default)
                    {
                        entity.Name = model.Name;
                        _context.Update(entity);
                    }
                }
                else
                {
                    var entity = new SectionModel
                    {
                        Id = model.Id,
                        CourseId = model.CourseId,
                        Name = model.Name,
                        ParentSectionId = model.ParentSectionId,
                        DisplayOrder = size + 1
                    };
                    _context.Add(entity);
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Edit), new { id = model.CourseId });
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult DeleteSection(long id, long courseId)
        {
            var sections = _context.Sections.Where(x => x.CourseId == courseId).ToList();
            var entity = _context.Sections.FirstOrDefault(x => x.Id == id);
            var children = _context.Sections.Where(x => x.ParentSectionId == id);

            if (entity != default)
            {
                RecursiveDelete(entity);
                _context.SaveChanges();
                 sections = _context.Sections.Where(x => x.CourseId == courseId && x.ParentSectionId == entity.ParentSectionId).ToList();             
                ReorderSections(sections);
                _context.SaveChanges();
            }    
            return RedirectToAction(nameof(Edit), new { id = courseId });
        }
        [HttpPost]
        public IActionResult DeleteItem(long id, long courseId, long sectionId)
        {
            var section = _context.Sections.Where(x => x.CourseId == courseId).FirstOrDefault(x => x.Id == sectionId);
            var entity = _context.MediaItems.Where(x => x.SectionId == section.Id).FirstOrDefault(x => x.Id == id);

            if (entity != default)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Edit), new { id = courseId });
        }

        private void RecursiveDelete(SectionModel parent)
        {
            var children = _context.Sections.Where(x => x.ParentSectionId == parent.Id);
            if (children.Any())
            {
                foreach (var child in children)
                {
                    RecursiveDelete(child);
                    _context.Remove(child);
                }
            }
            _context.Remove(parent);
        }

        private void ReorderSections(List<SectionModel> sections)
        {
            var order = 1;

            var orderedSections = sections.OrderBy(x => x.DisplayOrder).ToList();
            foreach(SectionModel section in orderedSections)
            {
                section.DisplayOrder = order;
                order++;
                _context.Sections.Attach(section);
                _context.Entry(section).State = EntityState.Modified;
            }
        }
    }
}

