using CleaningProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class ConfigureServiceEditModel
    {
        [Required]
        public int ServiceId { get; set; }
        public SelectList service { get; set; }

        [Required]
        public decimal ServiceCost { get; set; }

        [Required]
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }
        public string StartDate
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
           
        public int ServiceTypeId { get; set; }
        public SelectList ServiceType { get; set; }
    }
}
