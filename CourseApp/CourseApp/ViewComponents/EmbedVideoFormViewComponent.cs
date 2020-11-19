using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class EmbedVideoFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(long id, long courseId)
        {
            var model = new EmbedVideoVM
            {
                Id = id,
                CourseId = courseId,
            };
            return View(model);
        }
    }
}
