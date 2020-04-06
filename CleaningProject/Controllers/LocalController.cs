using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CleaningProject.Controllers
{
    public class LocalController : Controller
    {
        private UserManager<CleaningUser> userManager;
        private SignInManager<CleaningUser> signInManager;
        private IUserClaimsPrincipalFactory<CleaningUser> claimsPrincipalFactory;
        private ICompanyRepository CompanyRepository;
        private IEmailService EmailService;

        public LocalController(UserManager<CleaningUser> userManager, 
            IUserClaimsPrincipalFactory<CleaningUser> claimsPrincipalFactory, SignInManager<CleaningUser> signInManager,ICompanyRepository CompanyRepository,IEmailService EmailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
            this.CompanyRepository = CompanyRepository;
            this.EmailService = EmailService;
        }


        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Success = HttpContext.Session.GetString("Success");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            if (ModelState.IsValid)
            {
                var polly = CompanyRepository.GetCompany();

                var user = await userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    user = new CleaningUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Fullname = model.Fullname,
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Created = DateTime.Now
                    };
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");

                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationEmail = Url.Action("ConfirmEmailAddress", "Local",
                           new { token = token, email = user.Email }, Request.Scheme);

                        string confirm = "Please confirm your account by clicking this link: <a href=\""
                                               + confirmationEmail + "\">link</a>";

                        EmailService.Send(user.Email, user.Fullname, "Confirmation message", confirm);
                        // System.IO.File.WriteAllText("confirmation.txt", confirmationEmail);
                        ModelState.Clear();

                        HttpContext.Session.SetString("Success", "A link has been sent to your Email account for confirmation");

                        return RedirectToAction("Register");

                    }
                }
                else
                {
                    //user exist
                    ViewBag.AccountExist = "A link has been sent to your Email account for confirmation";
                    string confirm = "you attempted to register an email address that already exist on the system if you would like to login:<a href= \"" + Url.Action("Login", "Account") + "\">click here</a>";
                    EmailService.Send(user.Email, user.Fullname, "Account Exist", confirm);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View("Error");
        }

    }
}
