using CleaningProject.Models;
using CleaningProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleaningProject.Controllers
{
    public class CustomerController : Controller
    {
        private UserManager<CleaningUser> userManager;
        private IUserClaimsPrincipalFactory<CleaningUser> claimsPrincipalFactory;
        private SignInManager<CleaningUser> signInManager;
        private IEmailService EmailService;

        public CustomerController(UserManager<CleaningUser> userManager, IUserClaimsPrincipalFactory<CleaningUser> claimsPrincipalFactory,
            SignInManager<CleaningUser> signInManager,IEmailService EmailService)
        {
            this.userManager = userManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
            this.signInManager = signInManager;
            this.EmailService = EmailService;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            ViewBag.CustomerSuccess = HttpContext.Session.GetString("CustomerSuccess");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserModel model)
        {
            if (ModelState.IsValid)
            {
                var polly = await userManager.FindByEmailAsync(model.Email);
                if (polly == null)
                {
                    var user = new CleaningUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Fullname = model.Fullname,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.Email,
                        Created = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Customer");
                        ViewBag.CustomerSuccess = "A link has been sent to your Email account for confirmation";
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationEmail = Url.Action("ConfirmEmailAddress", "Customer",
                          new { token = token, email = user.Email }, Request.Scheme);

                        EmailService.Send(user.Email, user.Fullname, "Confirmation Message", "Please confirm your account by clicking this link: < a href =\""
                                               + confirmationEmail + "\">link</a>");


                        ModelState.Clear();
                        HttpContext.Session.SetString("CustomerSuccess", "A link has been sent to your Email account for confirmation");

                        return RedirectToAction("RegisterUser");
                    }
                }
                else
                {
                   ViewBag.CustomerExist= "A link has been sent to your Email account for confirmation";
                    string confirm = "you attempted to register an email address that already exist on the system if you would like to login:<a href= \"" + Url.Action("Login", "Account") + "\">click here</a>";
                    EmailService.Send(polly.Email, polly.Fullname, "Account Exist", confirm);
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
                    return RedirectToAction("LoginUser", "Account");
                }
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult ForgotUserPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotUserPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    ViewBag.Reset = "Your passsword reset is in progress check your email for further details";
                    string resetUrl = Url.Action("ResetUserPassword", "Customer",
                        new { token = token, email = user.Email }, Request.Scheme);

                    EmailService.Send(user.Email, user.Fullname, "Reset Password", "Please reset your password by clicking this link <a href=\""
                                               + resetUrl + "\">link</a>");
                }
                else
                {
                    ViewBag.Error = "Your passsword reset is in progress check your email for further details";
                    EmailService.Send(model.Email, "Anonymous", "Invalid Account", "You don't Have an account ");
                    //send dem a message to inform them that they dont have an email
                }
             
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetUserPassword(string token, string email)
        {
            return View(new ResetPasswordModel
            {
                Token = token,
                Email = email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetUserPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View();
                    }
                    if(await userManager.IsLockedOutAsync(user))
                    {
                        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                    }
                    EmailService.Send(user.Email, user.Fullname, "Password reset ", "You Password Reset is successfull");
                    ModelState.Clear();
                    //your pasword has been successfully reset
                    
                    return RedirectToAction("LoginUser");
                }
                ModelState.AddModelError(" ", "Invalid Request");
            }
            return View();
        }
    }
}