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
using System.Text.Json;
using System.Linq;
using System.Security.Claims;
using System;

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
        public IActionResult Cancel()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Receipt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public IActionResult isApproved([FromForm] string response)
        {
           if(!string.IsNullOrWhiteSpace(response))
            {
                var result = JsonSerializer.Deserialize<PaymentResponseVM>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (long.TryParse(result.CustomerId, out var transId))
                {
                    var transaction = _context.Transactions.FirstOrDefault(x => x.Id == transId);
                    if(transaction != null)
                    {
                        try
                        {
                            _context.Add(new UserCourseModel
                            {
                                UserId = transaction.UserId,
                                CourseId = transaction.CourseId
                            });
                            transaction.Response = response;
                            transaction.Status = "Success";
                            _context.Update(transaction);
                            _context.SaveChanges();
                            return RedirectToAction("Success");
                        }
                        catch(Exception e)
                        {
                            return RedirectToAction("Error", "Transaction");
                        }
                    }
                }              
            }
            return RedirectToAction("Error", "Transaction");           
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
                Amount = course.Price ?? 0,
                Status = "Pending"                
            };

            _context.Add(entity);
            _context.SaveChanges();

            entity = _context.Transactions.Include(x => x.Course).Include(x => x.User).FirstOrDefault(x => x.Id == entity.Id);
            var response = Services.GetAnAcceptPaymentPage.Run(entity, _settings, _urlHelper);
            model.SessionToken = response.sessionToken;
            model.Amount = entity.Amount;
            return View(model);
        }
    }
}
