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
    public class ConfigureTeamController : Controller
    {
        private IConfigureTeam ConfigureTeamRepository;
        private UserManager<CleaningUser> userManager;
        private ITeam TeamRepository;

        public ConfigureTeamController(IConfigureTeam ConfigureTeamRepository,UserManager<CleaningUser>userManager,ITeam TeamRepository)
        {
            this.ConfigureTeamRepository = ConfigureTeamRepository;
            this.userManager = userManager;
            this.TeamRepository = TeamRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateConfigureTeam()
        {
            ViewBag.ConfigureTeamSuccess = HttpContext.Session.GetString("ConfigureTeamSuccess");
            var po = userManager.Users.ToList();
            List<CleaningUser> pq = new List<CleaningUser>();
            foreach(var p in po)
            {
                if (await userManager.IsInRoleAsync(p, "Staff"))
                {
                    pq.Add(p);
                }
            }
            ConfigureEditModel cv = new ConfigureEditModel()
            {
                Staff= new SelectList(pq, "Id", "Fullname"),
                team= new SelectList(TeamRepository.GetAll(), "Id", "name")
            };
          
            return View(cv);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfigureTeam(ConfigureEditModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.StaffId);
              
                ConfigureTeam kj = new ConfigureTeam()
                {
                    Staff = user,
                    Team = TeamRepository.Get(model.TeamId)
                };
                ConfigureTeamRepository.Add(kj);
                ConfigureTeamRepository.Commit();

                HttpContext.Session.SetString("ConfigureTeamSuccess", "Team successfully configured");
                return RedirectToAction("CreateConfigureTeam");
            }
          
            var po = userManager.Users.ToList();
            List<CleaningUser> pq = new List<CleaningUser>();
            foreach (var p in po)
            {
                if (await userManager.IsInRoleAsync(p, "Staff"))
                {
                    pq.Add(p);
                }
            }
            ConfigureEditModel cv = new ConfigureEditModel()
            {
                Staff = new SelectList(pq, "Id", "Fullname"),
                team = new SelectList(TeamRepository.GetAll(), "Id", "name")
            };

            return View(cv);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ViewConfigureTeam(string sortOrder, string SearchString, string currentFilter, int? page)
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
            var kp = ConfigureTeamRepository.GetAll();

            var configTeam = from s in kp
                             select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                configTeam = configTeam.Where(s => s.Team.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    configTeam = configTeam.OrderByDescending(s => s.Team);
                    break;

                default:
                    configTeam = configTeam.OrderBy(s => s.Team);
                    break;
            }


            int pageSize = 3;

            return View(PaginatedList<ViewConfigureEditModel>.CreateAsync(configTeam, page ?? 1, pageSize));  
        }

        [HttpGet]
        public async Task<IActionResult> EditTeamConfigure(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var kp = ConfigureTeamRepository.Get(id);

            var po = userManager.Users.ToList();
            List<CleaningUser> pq = new List<CleaningUser>();
            foreach (var p in po)
            {
                if (await userManager.IsInRoleAsync(p, "Staff"))
                {
                    pq.Add(p);
                }
            }
            ConfigureEditModel cv = new ConfigureEditModel()
            {
                Staff = new SelectList(pq, "Id", "Fullname"),
                team = new SelectList(TeamRepository.GetAll(), "Id", "name")
            };

            return View(cv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeamAsync(ConfigureEditModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.StaffId);
                var kp = new ConfigureTeam()
                {
                    Id = id,
                    Team =  TeamRepository.Get(model.TeamId),
                    Staff = user
                };
                ConfigureTeamRepository.Update(kp);
                ConfigureTeamRepository.Commit();
                
               

                return RedirectToAction("ViewConfigureTeam");
            }
            List<CleaningUser> po = userManager.Users.ToList();
            List<CleaningUser> pq = new List<CleaningUser>();
            foreach (var p in po)
            {
                if (await userManager.IsInRoleAsync(p, "Staff"))
                {
                    pq.Add(p);
                }
            }

            ConfigureEditModel cv = new ConfigureEditModel()
            {
                Staff = new SelectList(pq, "Id", "Fullname"),
                team = new SelectList(TeamRepository.GetAll(), "Id", "name")
            };

            return View(cv);
         
        }


        [HttpGet]
        public IActionResult DeleteTeam(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var p=ConfigureTeamRepository.Get(id);
            ConfigureTeamRepository.Delete(p);
            ConfigureTeamRepository.Commit();

            return RedirectToAction("ViewConfigureTeam");
        }
    }
}
