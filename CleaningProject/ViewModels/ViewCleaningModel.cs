using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class ViewCleaningModel
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime SheduleDate { get; set; }
        public string TeamName { get; set; }
        public List<string> Staff { get; set; }
    }
}
