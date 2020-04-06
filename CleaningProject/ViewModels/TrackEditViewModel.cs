using CleaningProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleaningProject.ViewModels
{
    public class TrackEditViewModel
    {
        [Required]
        public int EquipmentId { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public int ServiceRequestId { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        public string EquipmentDate
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
        public SelectList Equip { get; set; }
        public SelectList job { get; set; }
        public SelectList Team { get; set; }
    }
}
