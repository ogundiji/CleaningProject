using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class ServiceEditModel
    {
        [Required]
        public string ServiceName { get; set; }


        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }


        public string ServiceDate
        {
            get
            {
                if (Date == null || Time ==null)
                {
                    return null;
                }
                else
                {
                    return string.Format("{0} {1}", Date, Time);
                }
            }
        }
    }
            
}
