using System.Collections.Generic;
using System.Linq;
using CleaningProject.Models;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleaningProject.Services
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private CleaningUserDbContext _context;

        public InvoiceRepository(CleaningUserDbContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void CreateInvoice(Invoice value)
        {
            _context.Add(value);
        }

        public IEnumerable<InvoiceViewModel> GetAll(string Id)
        {
            var po = _context.Invoice
                .Include(x=>x.Company)
                .Include(x=>x.ServiceRequest)
                .Include(x=>x.ServiceRequest.Customer)
                .Where(x => x.ServiceRequest.Customer.Id.Equals(Id))
                .ToList();
            List<InvoiceViewModel> op = new List<InvoiceViewModel>();
            foreach(var kl in po)
            {
                InvoiceViewModel km = new InvoiceViewModel()
                {
                    Id = kl.Id,
                    Logo = kl.Company.Logo,
                    CompanyAddress = kl.Company.Address,
                    CompanyPhone = kl.Company.PhoneNumber,
                    Customer = kl.ServiceRequest.Customer.Fullname,
                    Address = kl.ServiceRequest.Address,
                    Phone = kl.ServiceRequest.Phone,
                    InvoiceDate = kl.InvoiceDate,
                    InvoiceNo = kl.InvoiceNo,
                    Description = kl.description,
                    Quantity = kl.Quantity,
                    unitPrice = kl.unitPrice,
                    totalPrice = kl.totalPrice,
                    SubTotal = kl.SubTotal,
                    Total = kl.Total,
                    tax = kl.tax
                };
                op.Add(km);
            }

            return op;
        }

        public string GetUser(string Id)
        {
           return _context.Users.Find(Id).Fullname;
        }
      

        public string GetPhone(string Id)
        {
            return _context.Users.Find(Id).PhoneNumber;
        }

        [HttpGet]
        public InvoiceViewModel Get(int? id)
        {
            Invoice p = _context.Invoice
                .Include(x=>x.Company)
                .Include(x=>x.ServiceRequest)
                .Include(x=>x.ServiceRequest.Customer)
                .FirstOrDefault(x=>x.Id==id);

            InvoiceViewModel km = new InvoiceViewModel()
            {
                Id = p.Id,
                Logo = p.Company.Logo,
                CompanyAddress = p.Company.Address,
                CompanyPhone = p.Company.PhoneNumber,
                Customer = p.ServiceRequest.Customer.Fullname,
                Address = p.ServiceRequest.Address,
                Phone = p.ServiceRequest.Phone,
                InvoiceDate = p.InvoiceDate,
                InvoiceNo = p.InvoiceNo,
                Description = p.description,
                Quantity = p.Quantity,
                unitPrice = p.unitPrice,
                totalPrice = p.totalPrice,
                SubTotal = p.SubTotal,
                Total = p.Total,
                tax = p.tax
            };
            return km;
        }
    }
}
