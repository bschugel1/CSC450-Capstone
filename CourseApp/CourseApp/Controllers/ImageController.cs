using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CourseApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly IBlobStorageService _bbs;
        private readonly ApplicationContext _context;

        public ImageController(IBlobStorageService bss, ApplicationContext context)
        {
            this._bbs = bss;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Image(long id)
        {
            var entity = _context.Files.FirstOrDefault(x => x.Id == id);
            if(entity != default)
            {
                var stream = _bbs.DownloadFileFromBlob(entity.Uri, entity.MimeType);
                return File(stream, entity.MimeType, entity.Name);
            }

            return BadRequest();
        }

    }
   
}
