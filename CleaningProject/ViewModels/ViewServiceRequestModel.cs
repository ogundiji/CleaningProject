using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class ViewServiceRequestModel
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Service { get; set; }
        public string ServiceType{ get; set; }
        public DateTime SheduleDate { get; set; }
        public DateTime SheduleTime { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
    }

    public class RequestViewModel
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public DateTime SheduleDate { get; set; }
        public DateTime SheduleTime { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
    }
}
