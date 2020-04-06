using CleaningProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleaningProject.ViewModels
{
    public class ServiceRequestEditModel
    {
        public CleaningUser Customer { get; set; }

        [Required]
        public int ServiceID { get; set; }
        public SelectList Service { get; set; }

        [Required]
        public int ServiceTypeID { get; set; }
        public SelectList ServiceType { get; set; }

        [Required]
        public string SheduleDate { get; set; }

        [Required]
        public string SheduleTime { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        public List<DaysOfWeek> Days { get; set; }

        public string Status { get; set; }

        public string GetDate
        {
            get
            {
                if(SheduleDate == null || SheduleTime == null)
                {
                    return null;
                }
                else
                {
                    return string.Format("{0} {1}", SheduleDate, SheduleTime);
                }
            }
        }
       
       
       
    }

   
}
