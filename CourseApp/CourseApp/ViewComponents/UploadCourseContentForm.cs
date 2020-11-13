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
    public class UploadCourseContentFormViewComponent : ViewComponent
    {
        public UploadCourseContentFormViewComponent()
        {
        }

         public async Task<IViewComponentResult> InvokeAsync(long courseId, long? parentId)
        {
            var model = new SectionUploadVM {
                Id = 0,
                CourseId = courseId,
                ParentSectionId = parentId > 0 ? parentId : null
            };
            return View(model);
        }       
    }
}
