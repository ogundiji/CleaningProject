using CleaningProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleaningProject.ViewModels
{
    public class CleaningEditModel
    {
        [Required]
        public int ServiceRequestId { get; set; }
        public SelectList job { get; set; }

        [Required]
        public int TeamId { get; set; }

        public SelectList team { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        public string Created
        {
            get
            {
               if(Date==null || Time == null)
               {
                    return null;
               }
               else
               {
                    return string.Format("{0} {1}", Date, Time);
               }
            }
        }
           
        public string Status { get; set; }
      
    }
}
