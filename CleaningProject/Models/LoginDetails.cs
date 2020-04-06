using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;

namespace CleaningProject.Models
{
    public class LoginDetails:ActionFilterAttribute
    {
       // private UserManager<CleaningUser> _userManager;
       
        public override  void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home/index");
               
            }
            base.OnActionExecuting(context);
        }
    }
}
