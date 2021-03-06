﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using CourseApp.DAL;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CourseApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ApplicationContext context, IMapper mapper, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, ILoggerFactory loggerFactory, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _mapper.Map<UserModel>(model);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(model);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var signedUser = await _userManager.FindByEmailAsync(model.Email);

                if (signedUser == default)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(signedUser.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToLocal(returnUrl);

            }
        }

        [HttpGet]
        public async Task<IActionResult> Message()
        {
            return View(new MessageVM());
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return View(_mapper.Map<AccountVM>(user));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new { email = model.Email, token = token }, Request.Scheme);

                    _logger.Log(LogLevel.Warning, passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            // Fetch the userprofile
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View("EditFailure");
            }
            // Construct the viewmodel
            AccountEditVM model = new AccountEditVM();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.DisplayName = user.DisplayName;
            model.PhoneNumber = user.PhoneNumber;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountEditVM model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            string uniqueFileName = UploadedFile(model);

            if (user == null)
            {
                return View("EditFailure");
            }
            else
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DisplayName = model.DisplayName;
                user.PhoneNumber = model.PhoneNumber;
                user.ProfileImage = uniqueFileName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return View("EditConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        private string UploadedFile(AccountEditVM model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
