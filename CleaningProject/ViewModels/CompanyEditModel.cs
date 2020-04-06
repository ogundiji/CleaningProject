using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class CompanyEditModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public byte[] logo { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        public string companyCreated
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

       

    }
}
