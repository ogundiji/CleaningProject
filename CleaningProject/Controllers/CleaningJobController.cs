using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace CleaningProject.Controllers
{
    public class CleaningJobController : Controller
    {
        private ICleaning CleaningItemImp;
        private IRequestService ServiceRequestImp;
        private ITeam TeamRepository;

        public CleaningJobController(ICleaning CleaningItemImp,IRequestService ServiceRequestImp,ITeam TeamRepository)
        {
            this.CleaningItemImp = CleaningItemImp;
            this.ServiceRequestImp = ServiceRequestImp;
            this.TeamRepository = TeamRepository;
        }
      
        [HttpGet]
        public IActionResult CreateJob()
        {
            ViewBag.JobSuccess = HttpContext.Session.GetString("JobSuccess");
            CleaningEditModel p = new CleaningEditModel()
            {
                job = new SelectList(ServiceRequestImp.GetRequest(), "Id", "RequestName"),
                team = new SelectList(TeamRepository.GetAll(), "Id", "name")
            };
           return View(p);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateJob(CleaningEditModel model)
        {
            if (ModelState.IsValid)
            {

                if (CleaningItemImp.Exist(model.TeamId, model.ServiceRequestId))
                {
                    ViewBag.Message = "these job exist";
                }
                else
                {
                    var p = new CleaningItem()
                    {
                        ServiceRequest = ServiceRequestImp.Get(model.ServiceRequestId),
                        Team = TeamRepository.Get(model.TeamId),
                        Description = model.Description,
                        Created = DateTime.Parse(model.Created),
                        Status = "Assigned"
                    };

                    CleaningItemImp.Add(p);
                    CleaningItemImp.Commit();
                    ModelState.Clear();
                    HttpContext.Session.SetString("JobSucces", "Successfully Created a job");
                    return RedirectToAction("CreateJob");
                }
            }
            CleaningEditModel pq = new CleaningEditModel()
            {
                job = new SelectList(ServiceRequestImp.GetRequest(), "Id", "RequestName"),
                team = new SelectList(TeamRepository.GetAll(), "Id", "name")
            };
            return View(pq);
        }

        [HttpGet]
        public IActionResult ViewJob(string sortOrder,string SearchString,string currentFilter,int?page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }


            ViewData["CurrentFilter"] = SearchString;

            var mj = CleaningItemImp.GetAll();

            var CleanType = from s in mj
                              select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                CleanType = CleanType.Where(s => s.Customer.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    CleanType = CleanType.OrderByDescending(s => s.Customer);
                    break;
                default:
                    CleanType = CleanType.OrderBy(s => s.Customer);
                    break;
            }

            int pageSize = 3;

            return View(PaginatedList<ViewCleaningModel>.CreateAsync(CleanType, page ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult EditJob(int? id)
        {
           if (id == null)
           {
             return RedirectToAction("400");
           }
           var Clean = CleaningItemImp.Get(id);

            var qClean = new CleaningEditModel()
            {
                ServiceRequestId = Clean.ServiceRequest.Id,
                job = new SelectList(ServiceRequestImp.GetRequest(), "Id", "RequestName"),
                TeamId = Clean.Team.Id,
                team = new SelectList(TeamRepository.GetAll(), "Id", "name"),
                Description = Clean.Description,
                Date = Clean.Created.ToShortDateString(),
                Time = Clean.Created.ToShortTimeString()
            };
             
           return View(qClean);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditJob(CleaningEditModel model,int id)
        {
             if (ModelState.IsValid)
             {
               var k = new CleaningItem
               {
                  Id = id,
                  ServiceRequest = ServiceRequestImp.Get(model.ServiceRequestId),
                  Team = TeamRepository.Get(model.TeamId),
                  Description = model.Description,
                  Created = DateTime.Parse(model.Created),
                  Status = model.Status
               };

               CleaningItemImp.Update(k);
               CleaningItemImp.Commit();
               return RedirectToAction("ViewJob");
             }
            CleaningEditModel pq = new CleaningEditModel()
            {
                job = new SelectList(ServiceRequestImp.GetRequest(), "Id", "RequestName"),
                team = new SelectList(TeamRepository.GetAll(), "Id", "name")
            };
            return View();
        }

        [HttpGet]
        public IActionResult DeleteJob(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var m = CleaningItemImp.Get(id);
            CleaningItemImp.Delete(m);
            CleaningItemImp.Commit();

            return RedirectToAction("ViewJob");
        }
    }
}
