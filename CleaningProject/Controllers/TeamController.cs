using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleaningProject.Controllers
{
    public class TeamController : Controller
    {
        private ITeam TeamRepository;
      

        public TeamController(ITeam TeamRepository)
        {
            this.TeamRepository = TeamRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateTeam()
        {
            ViewBag.Success = HttpContext.Session.GetString("Success");
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTeam(TeamEditModel model)
        {
            if (ModelState.IsValid)
            {

                if (TeamRepository.TeamExist(model.name))
                {
                    ViewBag.Exist = "These Team account exist";
                }
                else
                {
                    var m = new Team()
                    {
                        name = model.name,
                        TeamDate = DateTime.Parse(model.TeamDate)
                    };
                    TeamRepository.Add(m);
                    TeamRepository.Commit();

                    HttpContext.Session.SetString("Success", "Team successfully created");

                    return RedirectToAction("CreateTeam");
                }
               
            }
              
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ViewTeam(string sortOrder,string SearchString,string currentFilter,int?page)
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

            var kp = TeamRepository.GetAll();

            var usr = from s in kp
                      select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                usr = usr.Where(s => s.name.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    usr = usr.OrderByDescending(s => s.name);
                    break;
                default:
                    usr = usr.OrderBy(s => s.name);
                    break;
            }

            int pageSize = 3;

            return View(PaginatedList<Team>.CreateAsync(usr, page ?? 1, pageSize));
         
        }

        [HttpGet]
        public IActionResult EditTeam(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var kp = TeamRepository.Get(id);
            TeamEditModel po = new TeamEditModel()
            {
                name = kp.name,
                Date = kp.TeamDate.ToShortDateString(),
                Time = kp.TeamDate.ToShortTimeString()
            };
            return View(po);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTeam(TeamEditModel model,int id)
        {
            if (ModelState.IsValid)
            {
                var jp = new Team()
                {
                    Id = id,
                    name = model.name,
                    TeamDate = DateTime.Parse(model.TeamDate)
                };
                TeamRepository.Update(jp);
                TeamRepository.Commit();
                return RedirectToAction("ViewTeam");
            }
           
            return View();
        }


        [HttpGet]
        public IActionResult DeleteTeam(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var k = TeamRepository.Get(id);
            TeamRepository.Delete(k);
            TeamRepository.Commit();

            return RedirectToAction("ViewTeam");
        }
    }
}