using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleaningProject.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private IService ServiceImp;

        public ServiceController(IService ServiceImp)
        {
            this.ServiceImp = ServiceImp;
        }

        [HttpGet]
        public IActionResult CreateService()
        {
            ViewBag.Service = HttpContext.Session.GetString("ServiceSuccess");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateService(ServiceEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (ServiceImp.TypeExist(model.ServiceName))
                {
                    ViewBag.ServiceExist = "These service exist";
                }
                else
                {
                    var k = new Service()
                    {
                        ServiceName = model.ServiceName,
                        ServiceDate=DateTime.Parse(model.ServiceDate)
                    };
                    ServiceImp.Add(k);
                    ServiceImp.Commit();
                    ModelState.Clear();


                    HttpContext.Session.SetString("ServiceSuccess", "Successfully Created a service");

                    return RedirectToAction("CreateService");
                }
                
            }
            return View();
        }

        [HttpGet]
        public  IActionResult ViewService(string sortOrder,string SearchString,string currentFilter,int? page)
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

            var k = ServiceImp.GetAll();

            var service = from s in k
                          select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                service = service.Where(s => s.ServiceName.Contains(SearchString) || s.ServiceDate.ToString().Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    service = service.OrderByDescending(s => s.ServiceName);
                    break;
                case "Date":
                    service = service.OrderBy(s => s.ServiceDate);
                    break;
                case "date_desc":
                    service = service.OrderByDescending(s => s.ServiceDate);
                    break;
                default:
                    service = service.OrderBy(s => s.ServiceName);
                    break;
            }

            int pageSize = 3;

            return View(PaginatedList<Service>.CreateAsync(service, page ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult EditService(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var k = ServiceImp.Get(id);
            ServiceEditModel p = new ServiceEditModel()
            {
                ServiceName = k.ServiceName,
                Date = k.ServiceDate.ToShortDateString(),
                Time = k.ServiceDate.ToShortTimeString()
            };
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditService(int id,ServiceEditModel model)
        {
            if (ModelState.IsValid)
            {
                var k = new Service()
                {
                    Id=id,
                    ServiceName = model.ServiceName,
                    ServiceDate= DateTime.Parse(model.ServiceDate)
                };
                ServiceImp.Update(k);
                ServiceImp.Commit();
                return RedirectToAction("ViewService");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteService(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var k = ServiceImp.Get(id);
            ServiceImp.Delete(k);
            ServiceImp.Commit();

            return RedirectToAction("ViewService");
        }

    }
}