using CourseApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(TransactionVM model)
        {

            CourseApp.Services.ChargeCardTest.Test("2yjMLaN53G", "6Dur886FBjN6HS83", model);

            return View();
        }
    }
}
