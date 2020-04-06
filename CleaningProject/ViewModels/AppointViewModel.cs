using CleaningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class AppointViewModel
    {
            public int CleaningItemId { get; set; }
            public string Customer { get; set; }
            public string Service { get; set; }
            public string Staff { get; set; }
            public string ServiceType { get; set; }
            public string Description { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Phone { get; set; }
            public List<DaysOfWeek> Days { get; set; }
            public string Status { get; set; }
            public DateTime ServiceDate { get; set; }
        
    }
}
