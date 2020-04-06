using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class ViewUserWithRole
    {
        public string Id { get; set; }
        public string fullname { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public List<string> UserRole { get; set; }
    }
}
