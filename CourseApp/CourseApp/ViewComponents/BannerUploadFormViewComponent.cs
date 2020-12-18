using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class BannerUploadFormViewComponent : ViewComponent
    {
        public BannerUploadFormViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(long courseId)
        {
            var model = new FileUploadVM
            {
                CourseId = courseId
            };
            return View(model);
        }
    }
}
