using System.Collections.Generic;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleaningProject.Controllers
{
    public class AppointmentController : Controller
    {
        private IAppointment AppointmentImpl;
        private UserManager<CleaningUser> userManager;
        private IInvoiceRepository InvoiceRepository;

        public AppointmentController(IAppointment AppointmentImpl,UserManager<CleaningUser>userManager,IInvoiceRepository InvoiceRepository
            ,IRequestService SeviceRequestImpl,ICompanyRepository CompanyRepository)
        {
            this.AppointmentImpl = AppointmentImpl;
            this.userManager = userManager;
            this.InvoiceRepository = InvoiceRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewAppoitment()
        {
            var user = User.Identity.Name;
            var cleaning = await userManager.FindByNameAsync(user);
            List<AppointViewModel> kop = AppointmentImpl.GetAppointment(cleaning.Id);
            return View(kop);
        }

        [HttpGet]
        [Authorize]
        public IActionResult UpdateAppointment(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var pk = AppointmentImpl.Get(id);
            pk.Status = "job in progress";
            AppointmentImpl.Update(pk);
            AppointmentImpl.Commit();
            return RedirectToAction("ViewAppoitment");
        }

        public IActionResult AppointmentDone(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }

            var pk = AppointmentImpl.Get(id);
            pk.Status = "job done";
            AppointmentImpl.Update(pk);
            AppointmentImpl.Commit();

            return RedirectToAction("ViewAppoitment");
        }
    }   
}
