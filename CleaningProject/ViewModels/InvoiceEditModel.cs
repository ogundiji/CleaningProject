using CleaningProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.ViewModels
{
    public class InvoiceEditModel
    {
        [Required]
        public int CompanyID { get; set; }
        public SelectList Company { get; set; }
        [Required]
        public string InvoiceDate { get; set; }
        [Required]
        public string InvoiceTime { get; set; }
        [Required]
        public int ServiceRequestID { get; set; }
        public SelectList ServiceRequest { get; set; }
        public string InvoiceNo { get; set; }
        public string ServiceDesc { get; set; }

        [Required]
        public decimal tax { get; set; }

        public string Date
        {
            get
            {
                if(InvoiceDate==null || InvoiceTime == null)
                {
                    return null;
                }
                else
                {
                    return string.Format("{0} {1}", InvoiceDate, InvoiceTime);
                }
            }
        }
           

    }
}
