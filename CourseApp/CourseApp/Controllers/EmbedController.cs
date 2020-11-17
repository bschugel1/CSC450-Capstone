using AutoMapper;
using CourseApp.DAL;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.Controllers
{
    public class EmbedController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<FileController> _logger;
        private readonly ApplicationContext _context;

        public EmbedController(IMapper mapper, ILogger<FileController> logger, ApplicationContext context)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> EmbedVideo(FileUploadVM model)
        {



            return RedirectToAction("Edit", "Author", new { id = model.CourseId });
        }
    }
}
