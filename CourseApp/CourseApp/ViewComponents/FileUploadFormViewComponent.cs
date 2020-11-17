using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class FileUploadFormViewComponent : ViewComponent
    {
        public FileUploadFormViewComponent()
        {
        }

         public async Task<IViewComponentResult> InvokeAsync(long id, long courseId, long? parentId)
        {
            var model = new FileUploadVM {
                Id = id,
                CourseId = courseId,
                ParentSectionId = parentId > 0 ? parentId : null
            };
            return View(model);
        }       
    }
}
