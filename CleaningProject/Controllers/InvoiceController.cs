using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace CleaningProject.Controllers
{
    public class InvoiceController : Controller
    {
        private ICompanyRepository CompanyRepository;
        private IRequestService ServiceRequestImpl;
        private IServiceConfig ConfigureService;
        private IService ServiceImp;
        private IInvoiceRepository InvoiceRepository;
        private UserManager<CleaningUser> userManager;
        private IDataProtector protector;
        

        public InvoiceController(ICompanyRepository CompanyRepository,IRequestService ServiceRequestImpl
            ,IServiceConfig ConfigureService,IService ServiceImp,IInvoiceRepository InvoiceRepository
            ,UserManager<CleaningUser>userManager,IDataProtectionProvider provider)
        {
            this.CompanyRepository = CompanyRepository;
            this.ServiceRequestImpl = ServiceRequestImpl;
            this.ConfigureService = ConfigureService;
            this.ServiceImp = ServiceImp;
            this.InvoiceRepository = InvoiceRepository;
            this.userManager = userManager;
            this.protector = provider.CreateProtector("protect_my_query_string");
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateInvoice()
        {
            ViewBag.InvoiceSuccess=HttpContext.Session.GetString("InvoiceSuccess");
            InvoiceEditModel invoice = new InvoiceEditModel()
            {
                Company = new SelectList(CompanyRepository.GetAll(), "Id", "name"),
                ServiceRequest = new SelectList(ServiceRequestImpl.GetRequest(), "Id", "RequestName")
            };
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult CreateInvoice(InvoiceEditModel model)
        {
            if (ModelState.IsValid)
            {
                var po = CompanyRepository.Get(model.CompanyID);
                var pk = ServiceRequestImpl.Get(model.ServiceRequestID);
                ServiceTypeRecord pq = ConfigureService.GetServiceType(pk.Service.Id, pk.ServiceType.Id);

                var qz = new Invoice()
                {
                    Company = po,
                    InvoiceDate = DateTime.Parse(model.Date),
                    ServiceRequest = pk,
                    InvoiceNo = GenarateInvoiceNo(),
                    description = pk.Service.ServiceName,
                    Quantity = 1,
                    unitPrice = pq.Price,
                    totalPrice = pq.Price,
                    SubTotal = pq.Price,
                    tax = model.tax,
                    Total = pq.Price + model.tax
                };

                InvoiceRepository.CreateInvoice(qz);
                InvoiceRepository.Commit();

                HttpContext.Session.SetString("InvoiceSuccess", "Invoice Successfully Created");

                return RedirectToAction("CreateInvoice");
            }
            var invoice = new InvoiceEditModel()
            {
                Company = new SelectList(CompanyRepository.GetAll(), "Id", "Fullname"),
                ServiceRequest = new SelectList(ServiceRequestImpl.GetRequest(), "Id", "RequestName")
            };
            return View(invoice);
        }

        [Authorize]
        public async Task<IActionResult> ViewInvoice()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var po = InvoiceRepository.GetAll(user.Id);
            List<InvoiceEncryptViewModel> lol = new List<InvoiceEncryptViewModel>();
            foreach(var j in po)
            {
                var k = new InvoiceEncryptViewModel()
                {
                    Id = this.protector.Protect(j.Id.ToString()),
                    Logo = j.Logo,
                    CompanyAddress = j.CompanyAddress,
                    CompanyPhone = j.CompanyPhone,
                    Customer = j.Customer,
                    Address = j.Address,
                    Phone = j.Phone,
                    InvoiceDate = j.InvoiceDate,
                    InvoiceNo = j.InvoiceNo,
                    Description = j.Description,
                    Quantity = j.Quantity,
                    unitPrice = j.unitPrice,
                    totalPrice = j.totalPrice,
                    SubTotal = j.SubTotal,
                    Total = j.Total,
                    tax = j.tax
                };
                lol.Add(k);
            }
            return View(lol);
        }

        [Authorize]
        public IActionResult InvoiceDetails(string id)
        {
            int orignalId = int.Parse(this.protector.Unprotect(id));
            var p = InvoiceRepository.Get(orignalId);

            return View(p);
            
           
        }

        [Authorize(Roles ="Customer")]
        public IActionResult InvoicePdf(int id)
        {
          
            var p = InvoiceRepository.Get(id);

            return new ViewAsPdf("InvoicePdf", p)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = { Left = 20, Bottom = -20, Top = 20, Right = 20 }
            };

        }

        public string GenarateInvoiceNo()
        {
            int maxSize = 8;
            char[] chars = new char[59];
            string a;
            a = "abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            return result.ToString();
        }
    }
}
