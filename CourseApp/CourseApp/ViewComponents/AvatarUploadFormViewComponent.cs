
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CourseApp.DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using CourseApp.Models;

namespace CourseApp.ViewComponents
{
    public class AvatarUploadFormViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<UserModel> _userManager;

        public AvatarUploadFormViewComponent(ApplicationContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new FileUploadVM
            {
                UserId = user.Id
            };
            return View(model);
        }
    }
}

