using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CleaningProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace CleaningProject.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<CleaningUser> userManager;

        public HomeController(UserManager<CleaningUser>userManager)
        {
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
           /* var po = User.Identity.Name;
            CleaningUser pk =await userManager.FindByNameAsync(po);
            ViewBag.Initial = GetInitial(pk.Fullname);
            ViewBag.Email = pk.Email;*/
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

       

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult HomePage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TwoFactor()
        {
            return View();
        }

        public string GetInitial(string value)
        {
            string output = "";
            string output2 = "";
            int k = 0;
            char[] polly = value.ToCharArray();
            for(int j = 0; j < polly.Length; j++)
            {
                if(polly[j]==' ')
                {
                    k += j;
                }
            }
            for (int j = 0; j < polly.Length; j++)
            {
                if (j > k)
                {
                    output2 += polly[j].ToString();
                }
                output += polly[j].ToString();
            }
            string result = string.Concat(output.Substring(0, 1),output2.Substring(0,1));

            return result;
        }
    }
}
