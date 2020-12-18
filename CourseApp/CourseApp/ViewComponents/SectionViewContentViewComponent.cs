﻿using AutoMapper;
using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class SectionViewContentViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public SectionViewContentViewComponent(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(SectionModel model)
        {
            var viewModel = new SectionVM
            {
                Id = model.Id,
                CourseId = model.CourseId,
                Items = _context.MediaItems.Where(x => x.SectionId == model.Id).ToList()
            };
            return View(viewModel);
        }
    }
}
