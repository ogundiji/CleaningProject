using System;
using System.Linq;
using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleaningProject.Controllers
{
    public class ConfigureServiceController : Controller
    {
        private IServiceConfig ConfigureService;
        private IService ServiceImp;
        private IServiceType ServiceTypeImp;

        public ConfigureServiceController(IServiceConfig ConfigureService,IService ServiceImp,IServiceType ServiceTypeImp)
        {
            this.ConfigureService = ConfigureService;
            this.ServiceImp = ServiceImp;
            this.ServiceTypeImp = ServiceTypeImp;
        }

        [HttpGet]
        public IActionResult CreateConfigureService()
        {
            ViewBag.successConfigure = HttpContext.Session.GetString("successConfigure");

            ConfigureServiceEditModel pl = new ConfigureServiceEditModel()
            {
                service = new SelectList(ServiceImp.GetAll(), "Id", "ServiceName"),
                ServiceType= new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType")
            };
            return View(pl);
        }
      
        [HttpPost]
        public IActionResult CreateConfigureService(ConfigureServiceEditModel value)
        {
            if (ModelState.IsValid)
            {
                if (ConfigureService.TypeExist(value.ServiceId, value.ServiceTypeId))
                {
                    ViewBag.Exist = "these account exist";
                }
                else
                {
                    int p = ConfigureService.MaxValue()+1;
                    var k = new ServiceTypeRecord()
                    {
                        Id=p,
                        ServiceId= value.ServiceId,
                        ServiceTypeId= value.ServiceTypeId,
                        ServiceCost = value.ServiceCost,
                        Discount = value.Discount,
                        Price = value.ServiceCost - (value.Discount / 100),
                        StartDate = DateTime.Parse(value.StartDate),
                        po=ServiceImp.Get(value.ServiceId),
                        qo=ServiceTypeImp.Get(value.ServiceTypeId)
                    };
                    ConfigureService.Add(k);
                    ConfigureService.Commit();
                    ModelState.Clear();


                    HttpContext.Session.SetString("successConfigure", "Successfully configured the service");

                    return RedirectToAction("CreateConfigureService");
                }
            }
            ConfigureServiceEditModel pl = new ConfigureServiceEditModel()
            {
                service = new SelectList(ServiceImp.GetAll(), "Id", "ServiceName"),
                ServiceType = new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType")
            };
            return View(pl);
        }

        [HttpGet]
        public IActionResult ViewConfiguredService(string sortOrder, string SearchString, string currentFilter, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "Type_desc" : "Type";

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }


            ViewData["CurrentFilter"] = SearchString;

            var k = ConfigureService.GetAll();

            var ServiceConfigure = from s in k
                              select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ServiceConfigure = ServiceConfigure.Where(s => s.po.ServiceName.Contains(SearchString) || s.qo.ServiceType.ToString().Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    ServiceConfigure = ServiceConfigure.OrderByDescending(s => s.po.ServiceName);
                    break;
                case "Type":
                    ServiceConfigure = ServiceConfigure.OrderBy(s => s.qo.ServiceType);
                    break;
                case "Type_desc":
                    ServiceConfigure = ServiceConfigure.OrderByDescending(s => s.qo.ServiceType);
                    break;
                default:
                    ServiceConfigure = ServiceConfigure.OrderBy(s => s.po.ServiceName);
                    break;
            }

            int pageSize = 3;

            return View(PaginatedList<ServiceTypeRecord>.CreateAsync(ServiceConfigure, page ?? 1, pageSize));
           
        }

        [HttpGet]
        public IActionResult EditConfiguredService(int?id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var k = ConfigureService.Get(id);
            ConfigureServiceEditModel pl = new ConfigureServiceEditModel()
            {
                ServiceId = k.ServiceId,
                service= new SelectList(ServiceImp.GetAll(), "Id", "ServiceName"),
                ServiceType= new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType"),
                ServiceTypeId = k.ServiceTypeId,
                ServiceCost = k.ServiceCost,
                Discount = k.Discount,
                Price = k.Price,
                Date = k.StartDate.ToShortDateString(),
                Time = k.StartDate.ToShortTimeString()
            };
            return View(pl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditConfiguredService(int id,ConfigureServiceEditModel model)
        {
            if (ModelState.IsValid)
            {
                var k = new ServiceTypeRecord()
                {
                    Id=id,
                    ServiceId = model.ServiceId,
                    ServiceTypeId = model.ServiceTypeId,
                    ServiceCost =model.ServiceCost,
                    Discount=model.Discount,
                    Price=model.ServiceCost-(model.Discount/100),
                    StartDate=DateTime.Parse(model.StartDate),
                    po = ServiceImp.Get(model.ServiceId),
                    qo = ServiceTypeImp.Get(model.ServiceTypeId)
                };
                ConfigureService.Update(k);
                ConfigureService.Commit();
                return RedirectToAction("ViewConfiguredService");
            }
            ViewBag.service = new SelectList(ServiceImp.GetAll(), "Id", "ServiceName");
            ViewBag.type = new SelectList(ServiceTypeImp.GetAll(), "Id", "ServiceType");
            return View();
        }

        [HttpGet]
        public IActionResult DeleteConfigureService(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var k = ConfigureService.Get(id);
            ConfigureService.Delete(k);
            ConfigureService.Commit();

            return RedirectToAction("ViewConfiguredService");
        }
    }
}
