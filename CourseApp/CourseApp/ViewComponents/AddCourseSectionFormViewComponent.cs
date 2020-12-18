using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseApp.ViewComponents
{
    public class AddCourseSectionFormViewComponent : ViewComponent
    {
        public AddCourseSectionFormViewComponent()
        {

        }

         public async Task<IViewComponentResult> InvokeAsync(long courseId, long? parentId)
        {
            var model = new SectionCreateVM {
                Id = 0,
                CourseId = courseId,
                ParentSectionId = parentId > 0 ? parentId : null
            };
            return View(model);
        }       
    }
}
