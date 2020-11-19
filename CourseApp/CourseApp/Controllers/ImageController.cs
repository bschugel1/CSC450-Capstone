using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.Models;
using CourseApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CourseApp.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }

    }
   
}
