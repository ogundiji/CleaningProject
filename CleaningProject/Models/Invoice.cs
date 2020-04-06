using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public DateTime InvoiceDate { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public string InvoiceNo { get; set; }
        public string description { get; set; }
        public int Quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal totalPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal tax { get; set; }

    }
}
