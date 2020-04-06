using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Controllers
{
    [CustomerAuthorize]
    public class ServiceARequestController : Controller
    {
        private IRequestService ServiceRequestImp;
        private IService ServiceImp;
        private IServiceType ServiceTypeImp;
        private UserManager<CleaningUser> userManager;
        private IDataProtector protector;

        public ServiceARequestController(IRequestService ServiceRequestImp, IService ServiceImp, IServiceType ServiceTypeImp, UserManager<CleaningUser> userManager, IDataProtectionProvider provider)
        {
            this.ServiceRequestImp = ServiceRequestImp;
            this.ServiceImp = ServiceImp;
            this.ServiceTypeImp = ServiceTypeImp;
            this.userManager = userManager;
            this.protector = provider.CreateProtector("protect_my_query_string");
        }

        [HttpGet]
        public IActionResult CreateRequest()
        {

            ViewBag.RequestSuccess = HttpContext.Session.GetString("RequestSuccess");
            ServiceRequestEditModel po = new ServiceRequestEditModel()
            {
                Days = GetDays(),
                Service = new SelectList(ServiceImp.GetAll(), "Id", "ServiceName"),
                ServiceType = new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType")
            };
          
            return View(po);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequest(ServiceRequestEditModel model)
        {
            if (ModelState.IsValid)
            {
                string output = "";
                foreach (DaysOfWeek p in model.Days)
                {
                    if (p.IsChecked == true)
                    {
                        output += p.name+",";
                    }

                }
                string name = User.Identity.Name;
                 var lp = await userManager.FindByNameAsync(name);
                 string Id = await userManager.GetUserIdAsync(lp);
                 var kp = new ServiceRequest()
                 {
                     Customer = lp,
                     Service = ServiceImp.Get(model.ServiceID),
                     ServiceType = ServiceTypeImp.Get(model.ServiceTypeID),
                     SheduleDate= DateTime.Parse(model.GetDate),
                     DaysOfWork= output,
                     Phone = model.Phone,
                     Address = model.Address,
                     City = model.City,
                     Status = "Not Approved",
                     RequestName = GetName(name)+"/"+model.SheduleDate.ToString()
                 };

                 ServiceRequestImp.Add(kp);

                 ServiceRequestImp.Commit();


                 ModelState.Clear();

                 HttpContext.Session.SetString("RequestSuccess", "Cleaning Request Successful");

                 return RedirectToAction("CreateRequest");
            }
            ServiceRequestEditModel po = new ServiceRequestEditModel()
            {
                Days = GetDays(),
                Service = new SelectList(ServiceImp.GetAll(), "Id", "ServiceName"),
                ServiceType = new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType")
            };

            return View(po);
        }

          [HttpGet]
          public async Task<IActionResult> ViewRequest()
          {
               string name = User.Identity.Name;
               var lp = await userManager.FindByNameAsync(name);
               string Id = await userManager.GetUserIdAsync(lp);
               List<RequestViewModel> mk = new List<RequestViewModel>();
               var p = ServiceRequestImp.GetAll(Id).ToList();
               foreach(var m in p)
               {
                   RequestViewModel po = new RequestViewModel()
                   {
                       Id=this.protector.Protect(m.Id.ToString()),
                       Customer = lp.Fullname,
                       Service = m.Service.ServiceName,
                       ServiceType = m.ServiceType.ServiceType,
                       SheduleDate = m.SheduleDate,
                       Phone = m.Phone,
                       Address = m.Address,
                       City = m.City,
                       Status = m.Status
                   };
                   mk.Add(po);
               }
               var mp = from s in mk
                       select s;

               return View(mp);
            
          }

        [HttpGet]
        public IActionResult ApproveRequest(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var q = ServiceRequestImp.Get(id);
            q.Status = "Approved";
            ServiceRequestImp.Update(q);
            ServiceRequestImp.Commit();

            return RedirectToAction("ViewCustomerRequest");

        }

        [HttpGet]
        public IActionResult ViewCustomerRequest()
        {
            return View(ServiceRequestImp.GetService());
        }

        [HttpGet]
        public IActionResult EditRequest(string id)
        {
            var ID = int.Parse(this.protector.Unprotect(id));
            var hj = ServiceRequestImp.Get(ID);
            ServiceRequestEditModel hjk = new ServiceRequestEditModel()
            {
                Customer = hj.Customer,
                ServiceID = hj.Service.Id,
                ServiceTypeID = hj.ServiceType.Id,
                SheduleDate = GetMyDate(hj.SheduleDate),
                SheduleTime = GetMyTime(hj.SheduleDate),
                Phone = hj.Phone,
                Address = hj.Address,
                City = hj.City,
                Days = FilterDays(hj.DaysOfWork),
                Status="Resheduled",
                Service = new SelectList(ServiceImp.GetAll(), "Id", "ServiceName"),
                ServiceType= new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType")
            };
           
            return View(hjk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRequest(string id, ServiceRequestEditModel model)
        {

           if (ModelState.IsValid)
           {
                string output = "";
                foreach (DaysOfWeek p in model.Days)
                {
                    if (p.IsChecked == true)
                    {
                        output += p.name + ",";
                    }

                }
                 var k = new ServiceRequest()
                 {
                     Id = int.Parse(this.protector.Unprotect(id)),
                     Customer = model.Customer,
                     Service = ServiceImp.Get(model.ServiceID),
                     ServiceType = ServiceTypeImp.Get(model.ServiceTypeID),
                     SheduleDate = DateTime.Parse(model.GetDate),
                     Address = model.Address,
                     Phone = model.Phone,
                     City = model.City,
                     DaysOfWork=output,
                     Status = "Rescheduled"
                 };
                 ServiceRequestImp.Update(k);
                 ServiceRequestImp.Commit();
                 ModelState.Clear();
                 return RedirectToAction("ViewRequest");
           }
            ServiceRequestEditModel po = new ServiceRequestEditModel()
            {
                Service = new SelectList(ServiceImp.GetAll(), "Id", "ServiceName"),
                ServiceType = new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType")
            };
            return View(po);
        }

        [HttpGet]
        public IActionResult DeleteRequest(string id)
        {
            var ID = int.Parse(this.protector.Unprotect(id));
            var k = ServiceRequestImp.Get(ID);
            ServiceRequestImp.Delete(k);
            ServiceRequestImp.Commit();

            return RedirectToAction("ViewRequest");
        }

        public string GetName(string name)
        {
            char[] poly = name.ToCharArray();
            string output = "";
            foreach (var k in poly)
            {
                if (k == '@')
                {
                    break;
                }
                else
                {
                    output += k.ToString();
                }
            }
            return output;
        }
        public async Task GetCustomerAsync(string Id)
        {
            var p = await userManager.FindByIdAsync(Id);
            string q = p.Fullname;

        }

        public List<DaysOfWeek> GetDays()
        {
            var k = new List<DaysOfWeek>
                {
                    new DaysOfWeek{Id=1,name="Monday",IsChecked=false},
                    new DaysOfWeek{Id=2,name="Tuesday",IsChecked=false},
                    new DaysOfWeek{Id=3,name="Wednesday",IsChecked=false},
                    new DaysOfWeek{Id=4,name="Thursday",IsChecked=false},
                    new DaysOfWeek{Id=5,name="Friday",IsChecked=false},
                    new DaysOfWeek{Id=6,name="Saturday",IsChecked=false},
                    new DaysOfWeek{Id=7,name="Sunday",IsChecked=false}
                };

            return k;
        }

        public List<DaysOfWeek> FilterDays(string model)
        {
            List<string> result = SplitDays(model);
            List<DaysOfWeek> op = GetDays();
            foreach(var p in op)
            {
                if (result.Contains(p.name))
                {
                    p.IsChecked = true;
                }
            }

            return op;
        }
        public List<string>SplitDays(string value)
        {
            List<string> lol = new List<string>();
            var p = value.Split(",");
            foreach(var z in p)
            {
                lol.Add(z);
            }
            return lol;
        }
        public string GetMyDate(DateTime c)
        {
            return c.ToShortDateString();
        }
        public string GetMyTime(DateTime c)
        {
            return c.ToShortTimeString();
        }

    }
}
