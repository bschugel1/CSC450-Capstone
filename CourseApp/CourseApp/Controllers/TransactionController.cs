using AutoMapper;
using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.Models.Configuration;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;

namespace CourseApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUrlHelper _urlHelper;
        private readonly AuthorizeNetPaymentSettings _settings;

        public TransactionController(ApplicationContext context, IMapper mapper, ILogger<TransactionVM> logger, IOptions<AuthorizeNetPaymentSettings> settings, IUrlHelper urlHelper)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _urlHelper = urlHelper;
            _settings = settings.Value;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Checkout(long id)
        {
            var model = new TransactionVM
            {
                CourseId = id

            };

            var course = _context.Courses.FirstOrDefault(x => x.Id == model.CourseId);
            var entity = new TransactionModel
            {
                Time = System.DateTime.Now,
                CourseId = course.Id,
                UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Amount = course.Price ?? 0
            };

            _context.Add(entity);
            _context.SaveChanges();

            
            entity = _context.Transactions.Include(x => x.Course).Include(x => x.User).FirstOrDefault(x => x.Id == entity.Id);

            var response = Services.GetAnAcceptPaymentPage.Run(entity, _settings, _urlHelper);
            model.SessionToken = response.sessionToken;
            return View(model);
        }
    }
}
