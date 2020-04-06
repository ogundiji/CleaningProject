using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CleaningProject.Controllers
{
    public class ServiceTypeController : Controller
    {
        private IServiceType ServiceTypeImp;

        public ServiceTypeController(IServiceType ServiceTypeImp)
        {
            this.ServiceTypeImp = ServiceTypeImp;
        }

        [HttpGet]
        public IActionResult CreateServiceType()
        {
            ViewBag.ServiceTypeSuccess = HttpContext.Session.GetString("ServiceTypeSuccess");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateServiceType(ServiceTypeEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (ServiceTypeImp.TypeExist(model.ServiceType))
                {
                    ViewBag.ServiceTypeExist = "These service exist";
                }
                else
                {
                    var k = new ServiceRecord()
                    {
                        ServiceType = model.ServiceType,
                        ServiceTypeDate=DateTime.Parse(model.ServiceTypeDate)
                    };
                    ServiceTypeImp.Add(k);
                    ServiceTypeImp.Commit();
                    ModelState.Clear();
                    

                    HttpContext.Session.SetString("ServiceTypeSuccess", "Service successfully created");

                    return RedirectToAction("CreateServiceType");
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult ViewServiceType(string sortOrder,string SearchString,string currentFilter,int?page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }


            ViewData["CurrentFilter"] = SearchString;

            var m = ServiceTypeImp.GetAll();

            var ServiceType = from s in m
                              select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ServiceType = ServiceType.Where(s => s.ServiceType.Contains(SearchString) || s.ServiceTypeDate.ToString().Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    ServiceType = ServiceType.OrderByDescending(s => s.ServiceType);
                    break;
                case "Date":
                    ServiceType = ServiceType.OrderBy(s => s.ServiceType);
                    break;
                case "date_desc":
                    ServiceType = ServiceType.OrderByDescending(s => s.ServiceTypeDate);
                    break;
                default:
                    ServiceType = ServiceType.OrderBy(s => s.ServiceType);
                    break;
            }

            int pageSize = 3;

            return View(PaginatedList<ServiceRecord>.CreateAsync(ServiceType, page ?? 1, pageSize));
           
        }

        [HttpGet]
        public IActionResult EditServiceType(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var k = ServiceTypeImp.Get(id);
            ServiceTypeEditModel po = new ServiceTypeEditModel()
            {
                ServiceType = k.ServiceType,
                Date = k.ServiceTypeDate.ToShortDateString(),
                Time = k.ServiceTypeDate.ToShortTimeString()
            };
            return View(po);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditServiceType(int id, ServiceTypeEditModel model)
        {
            if (ModelState.IsValid)
            {
                var k = new ServiceRecord()
                {
                    Id=id,
                    ServiceType = model.ServiceType,
                    ServiceTypeDate=DateTime.Parse(model.ServiceTypeDate)
                };
                ServiceTypeImp.Update(k);
                ServiceTypeImp.Commit();
                return RedirectToAction("ViewServiceType");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteServiceType(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var k = ServiceTypeImp.Get(id);
            ServiceTypeImp.Delete(k);
            ServiceTypeImp.Commit();

            return RedirectToAction("ViewServiceType");
        }


    }
}