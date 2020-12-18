using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class EmbedVideoFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(long id, long courseId, long? parentId)
        {
            var model = new EmbedVideoVM
            {
                Id = id,
                CourseId = courseId,
                ParentSectionId  = parentId
            };
            return View(model);
        }
    }
}
