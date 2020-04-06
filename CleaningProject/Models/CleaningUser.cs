using Microsoft.AspNetCore.Identity;
using System;

namespace CleaningProject.Models
{
    public class CleaningUser:IdentityUser
    {
        public string Fullname { get; set; }
        public DateTime Created { get; set; }
    }
}
