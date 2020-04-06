using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Controllers
{
    public class AccountController : Controller
    {
     
            private UserManager<CleaningUser> userManager;
            private IUserClaimsPrincipalFactory<CleaningUser> claimPrincipalFactory;
            private SignInManager<CleaningUser> signInManager;
            private RoleManager<IdentityRole> roleManager;
            private IUserImage UserImageImp;
            private IEmailService EmailService;

        public AccountController(UserManager<CleaningUser> userManager, IUserClaimsPrincipalFactory<CleaningUser> claimsPrincipalFactory,
                SignInManager<CleaningUser> signInManager,RoleManager<IdentityRole>roleManager,IUserImage UserImageImp,
                IEmailService EmailService)
            {
                this.userManager = userManager;
                this.claimPrincipalFactory = claimsPrincipalFactory;
                this.signInManager = signInManager;
                this.roleManager = roleManager;
                this.UserImageImp = UserImageImp;
                this.EmailService = EmailService;
            }

            [HttpGet]
            public IActionResult RegisterStaff()
            {
                ViewBag.role = GetUserRole();
                ViewBag.Success = HttpContext.Session.GetString("Success");
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RegisterStaff(RegisterStaffModel model, List<IFormFile> UserImage)
            {

                if (ModelState.IsValid)
                {
                    var polly = await userManager.FindByEmailAsync(model.Email);
                    if (polly == null)
                    {
                        IFormFile f = UserImage.FirstOrDefault();
                        if (f.Length > 0)
                        {
                            if (IsImage(f))
                            {
                                byte[] imageData = null;
                                using (var binary = new MemoryStream())
                                {
                                    await f.CopyToAsync(binary);
                                    imageData = binary.ToArray();
                                }
                                var user = new CleaningUser
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Fullname = model.Fullname,
                                    Email = model.Email,
                                    PhoneNumber = model.PhoneNumber,
                                    UserName = model.Email,
                                    Created = DateTime.Now
                                };
                                var k = new UserImageRecord
                                {
                                    Staff=user,
                                    Upload = imageData
                                };
                                var result = await userManager.CreateAsync(user, model.Password);
                                UserImageImp.AddUserImage(k);
                                
                                if (result.Succeeded)
                                {
                                    await userManager.AddToRoleAsync(user, model.UserRoles);
                                    
                                    HttpContext.Session.SetString("Success", "Registration in Progress,check your email to complete the registration");
                                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                                    string confirmationEmail = Url.Action("ConfirmEmailAddress", "Local",
                                       new { token, email = user.Email }, Request.Scheme);
                                     string confirm = "Please confirm your account by clicking this link: <a href=\""
                                         + confirmationEmail + "\">link</a>";

                                    EmailService.Send(user.Email, user.Fullname, "Confirmation message", confirm);
                                   
                                    ModelState.Clear();

                                   return RedirectToAction("RegisterStaff");
                                }
                               
                            }
                            else
                            {
                                ViewBag.NotRightFormat = "you have not uploaded an image";
                            }
                        }
                        else
                        {
                            ViewBag.noImage = "you have not uploaded an image";
                        }
                    }
                    else
                    {
                           ViewBag.Exist = "Registration in Progress,check your email to complete the registration";
                           string confirm = "you attempted to register an email address that already exist on the system if you would like to login:<a href= \"" + Url.Action("Login", "Account") + "\">click here</a>";
                           EmailService.Send(polly.Email, polly.Fullname, "Account Exist", confirm);
                    }

                }
                ViewBag.role = GetUserRole();
                return View();
            }

            [HttpGet]
            public IActionResult LoginUser()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> LoginUser(LoginModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByNameAsync(model.UserName);
                    if (user != null && !await userManager.IsLockedOutAsync(user))
                    {
                       if (await userManager.CheckPasswordAsync(user, model.Password))
                       {
                         if (!await userManager.IsEmailConfirmedAsync(user))
                         {
                            ModelState.AddModelError("", "Email is not confimed");
                            return View();
                         }
                         await userManager.ResetAccessFailedCountAsync(user);
                         ViewBag.name = user.Fullname;
                         var principal = await claimPrincipalFactory.CreateAsync(user);
                         await HttpContext.SignInAsync("Identity.Application", new System.Security.Claims.ClaimsPrincipal(principal));

                         return RedirectToAction("Index", "Home");
                       }
                       await userManager.AccessFailedAsync(user);
                       if(await userManager.IsLockedOutAsync(user))
                       {

                        EmailService.Send(user.Email, user.Fullname, "Account Lockout", "Your Account has been locked <a href= \"" + Url.Action("ForgotUserPassword", "Customer") + "\">click here</a> to Unlock it");
                         //notify them to reset there password
                       }
                       
                    }
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
                return View();
            }


            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(LoginModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByNameAsync(model.UserName);

                    if (user != null && !await userManager.IsLockedOutAsync(user))
                    {
                        if(await userManager.CheckPasswordAsync(user, model.Password))
                      {
                        if (!await userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", "Email is not confimed");
                            return View();
                        }
                        //after successful login it will reset there locked Account
                        await userManager.ResetAccessFailedCountAsync(user);


                        var principal = await claimPrincipalFactory.CreateAsync(user);
                        await HttpContext.SignInAsync("Identity.Application", new System.Security.Claims.ClaimsPrincipal(principal));

                        return RedirectToAction("Index", "Home");
                      }

                        await userManager.AccessFailedAsync(user);

                        if(await userManager.IsLockedOutAsync(user))
                        {
                         EmailService.Send(user.Email, user.Fullname, "Account Lockout", "Your Account has been locked <a href= \"" 
                             + Url.Action("ForgotPassword", "Account") + "\">click here</a> to Unlock it");
                        //send them email notify them to reset there password
                        }
                    }
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
                return View();
            }

            [HttpGet]
            public async Task<IActionResult> SignOut()
            {

                string Id = null;
                foreach (var p in User.Claims)
                {
                    if (p.Type.Equals("Id"))
                    {
                        Id = p.Value.ToString();
                    }
                }
                var user = await userManager.FindByIdAsync(Id);
                IList<string> r = await userManager.GetRolesAsync(user);
                string role = r[0];
                await signInManager.SignOutAsync();
                if (role.Equals("Customer"))
                {
                    return RedirectToAction("LoginUser", "Account");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }

        [HttpGet]
        public async Task<IActionResult> ViewUser(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "Email_desc" : "Email";

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }


            ViewData["CurrentFilter"] = SearchString;

            var UserRoles = new List<ViewUserWithRole>();
            var UsersInfo = userManager.Users.ToList();
            foreach (var k in UsersInfo)
            {
                var kp = new ViewUserWithRole
                {
                    Id = k.Id,
                    fullname = k.Fullname,
                    Email = k.Email,
                    phone = k.PhoneNumber
                };
                UserRoles.Add(kp);
            }

            foreach (var po in UserRoles)
            {
                var user = userManager.Users.FirstOrDefault(x => x.Email.Equals(po.Email));
                IList<string> kpo = await userManager.GetRolesAsync(user);
                List<string> kj = kpo.ToList();
                po.UserRole = kj;
            }
              List<ViewUserWithRole> FilterRole = new List<ViewUserWithRole>();
              foreach(var j in UserRoles)
              {
                  if (!j.UserRole[0].Equals("Admin"))
                  {
                      FilterRole.Add(j);
                  }
              }

            var usr = from s in FilterRole
                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                usr = usr.Where(s => s.fullname.Contains(SearchString) || s.Email.ToString().Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    usr = usr.OrderByDescending(s => s.fullname);
                    break;
                case "Email":
                    usr = usr.OrderBy(s => s.Email);
                    break;
                case "Email_desc":
                    usr = usr.OrderByDescending(s => s.Email);
                    break;
                default:
                    usr = usr.OrderBy(s => s.fullname);
                    break;
            }



            int pageSize = 3;

            return View(PaginatedList<ViewUserWithRole>.CreateAsync(usr, page ?? 1, pageSize));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
                if (id == null)
                {
                    return RedirectToAction("Error");
                }
                CleaningUser user = await userManager.FindByIdAsync(id);
                await userManager.DeleteAsync(user);

                return RedirectToAction("ViewUser", "Account");

        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var token =await userManager.GeneratePasswordResetTokenAsync(user);
                       var resetUrl = Url.Action("ResetPassword", "Account",
                            new { token, email = user.Email }, Request.Scheme);
                       System.IO.File.WriteAllText("ResetPassword.txt", resetUrl);

                      ViewBag.Reset = "Your passsword reset is in progress check your email for further deatails";

                    }
                    else
                    {
                       //send the user of the email these message
                       //send dem a message to inform them that they dont have an email
                       ViewBag.Reset = "Your passsword reset is in progress check your email for further deatails";
                      
                    }
                    
                    ModelState.Clear();
                }
                return View();
            }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
            {
                return View(new ResetPasswordModel
                {
                    Token = token,
                    Email = email
                });
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
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
                        ModelState.Clear();

                        //send the user message that the password has been reset

                        return RedirectToAction("Login");
                    }
                    ModelState.AddModelError(" ","invalid request");
                }
                return View();
        }

       public List<SelectListItem> GetUserRole()
       {
            List<SelectListItem> r = new List<SelectListItem>();
            var kpo = roleManager.Roles.ToList();
            List<string> name_of_role = new List<string>();
            foreach (var k in kpo)
            {
                if (!(k.Name.Equals("Admin")))
                {
                    name_of_role.Add(k.Name.ToString());
                }
            }

            foreach (var b in name_of_role)
            {
                r.Add(new SelectListItem
                {
                    Value = b,
                    Text = b
                });
            }

             return r;

       }
       public bool IsImage(IFormFile file)
       {
            
            if (file.ContentType.Contains("image"))
            { 
                if (ImageFormat(file))
                {
                    return true;
                }
            }

             return false;
        }

        public bool ImageFormat(IFormFile file)
        {
            bool isFormat = false;
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like..
            foreach (var item in formats)
            {
                if (file.FileName.Contains(item))
                {
                    isFormat = true;
                }
            }

            return isFormat;
        }

    }
}
    