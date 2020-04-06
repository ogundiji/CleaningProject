using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleaningProject.Models
{
    public class CleaningUserClaimsPrincipalFactory:UserClaimsPrincipalFactory<CleaningUser, IdentityRole>
    {
        public CleaningUserClaimsPrincipalFactory(UserManager<CleaningUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
           
        }
        protected async override Task<ClaimsIdentity> GenerateClaimsAsync(CleaningUser user)
        {
            string pol = user.Id;
            string name = user.Fullname;
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Id", pol));
            identity.AddClaim(new Claim("Name", GetInitial(name)));
            identity.AddClaim(new Claim("Email", user.Email));

            return identity;
        }

        public string GetInitial(string value)
        {
            string output = "";
            string output2 = "";
            int k = 0;
            char[] polly = value.ToCharArray();
            for (int j = 0; j < polly.Length; j++)
            {
                if (polly[j] == ' ')
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
            string result = string.Concat(output.Substring(0, 1), output2.Substring(0, 1));

            return result;
        }
    }
}
