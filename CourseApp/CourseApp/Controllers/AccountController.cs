﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly IMapper _mapper;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger _logger;

        public AccountController(IMapper mapper, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, ILoggerFactory loggerFactory)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

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

            // await _userManager.AddToRoleAsync(user, "Guest");

            return RedirectToAction(nameof(HomeController.Index), "Home");            
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


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if(returnUrl == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToLocal(returnUrl);

            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> PasswordRecovery(string returnUrl = null)
        {





            return RedirectToLocal(returnUrl);
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

    }
}
