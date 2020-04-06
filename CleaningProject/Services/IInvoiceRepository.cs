using CleaningProject.Models;
using CleaningProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IInvoiceRepository
    {
        void CreateInvoice(Invoice value);
        void Commit();
        IEnumerable<InvoiceViewModel> GetAll(string Id);
        InvoiceViewModel Get(int?id);
    }
}

