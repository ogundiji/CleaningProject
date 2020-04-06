using System;
using System.Linq;
using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleaningProject.Controllers
{
    public class EquipmentController : Controller
    {
        private IEquipment EquipmentRepository;

        public EquipmentController(IEquipment EquipmentRepository)
        {
            this.EquipmentRepository = EquipmentRepository;
        }


        [HttpGet]
        public IActionResult CreateEquipment()
        {
            ViewBag.Equipment = HttpContext.Session.GetString("Equipment");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEquipment(EquipmentEditModel e)
        {
            if (ModelState.IsValid)
            {
                if (EquipmentRepository.Exist(e.EquipmentName))
                {
                    ViewBag.EquipmentExist = "The Equipment Exist";
                }
                else
                {
                    var k = new Equipment
                    {
                        EquipmentName = e.EquipmentName,
                        EquipmentType = e.EquipmentType,
                        EquipmentDate = DateTime.Parse(e.EquipmentDate)
                    };
                    EquipmentRepository.Add(k);
                    EquipmentRepository.Commit();
                   

                    HttpContext.Session.SetString("Equipment", "Successfully created these equipment");
                    return RedirectToAction("CreateEquipment");
                }
               
            }
            return View();
        }

        [HttpGet]
        public IActionResult ViewEquipment(string sortOrder, string SearchString, string currentFilter, int? page)
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
            var kp = EquipmentRepository.GetAll();

            var equipment = from s in kp
                             select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                equipment = equipment.Where(s => s.EquipmentName.Contains(SearchString) || s.EquipmentDate.ToString().Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    equipment = equipment.OrderByDescending(s => s.EquipmentName);
                    break;
                case "Date":
                    equipment = equipment.OrderBy(s => s.EquipmentDate);
                    break;
                case "date_desc":
                    equipment = equipment.OrderByDescending(s => s.EquipmentDate);
                    break;
                default:
                    equipment = equipment.OrderBy(s => s.EquipmentName);
                    break;
            }


            int pageSize = 3;
            return View(PaginatedList<Equipment>.CreateAsync(equipment, page ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult EditEquipment(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var p = EquipmentRepository.Get(id);
            EquipmentEditModel pk = new EquipmentEditModel()
            {
                EquipmentName = p.EquipmentName,
                EquipmentType = p.EquipmentType,
                Date = p.EquipmentDate.ToShortDateString(),
                Time = p.EquipmentDate.ToShortTimeString()
            };

            return View(pk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEquipment(EquipmentEditModel val, int id)
        {
            if (ModelState.IsValid)
            {
                var p = new Equipment
                {
                    Id=id,
                    EquipmentName = val.EquipmentName,
                    EquipmentType = val.EquipmentType,
                    EquipmentDate = DateTime.Parse(val.EquipmentDate)
                };
                EquipmentRepository.update(p);
                EquipmentRepository.Commit();

                return RedirectToAction("ViewEquipment");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteEquipment(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var p = EquipmentRepository.Get(id);
            EquipmentRepository.delete(p);
            EquipmentRepository.Commit();

            return RedirectToAction("ViewEquipment");

        }
    }
}