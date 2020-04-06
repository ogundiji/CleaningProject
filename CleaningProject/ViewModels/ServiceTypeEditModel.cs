using System;
using System.ComponentModel.DataAnnotations;

namespace CleaningProject.ViewModels
{
    public class ServiceTypeEditModel
    {
        [Required]
        public string ServiceType { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }
        public string ServiceTypeDate
        {
            get
            {
                if(Date==null || Time == null)
                {
                    return null;
                }
                else
                {
                   return  string.Format("{0} {1}", Date, Time);
                }
            }
        }
            
    }
}
