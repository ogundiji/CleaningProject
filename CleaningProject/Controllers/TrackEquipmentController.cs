using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace CleaningProject.Controllers
{
    public class TrackEquipmentController : Controller
    {
        private IEquipmentTracking EquipmentTrackingImp;
        private ITeam TeamRepository;
        private IEquipment EquipmentRepository;
        private IRequestService ServiceRequestImp;

        public TrackEquipmentController(IEquipmentTracking EquipmentTrackingImp,ITeam TeamRepository,IEquipment EquipmentRepository,IRequestService ServiceRequestImp)
        {
            this.EquipmentTrackingImp = EquipmentTrackingImp;
            this.TeamRepository = TeamRepository;
            this.EquipmentRepository = EquipmentRepository;
            this.ServiceRequestImp = ServiceRequestImp;
        }
        [HttpGet]
        public IActionResult CreateTrack()
        {
            ViewBag.EquipmentTrackSuccess = HttpContext.Session.GetString("EquipmentTrackSuccess");
            TrackEditViewModel cu = new TrackEditViewModel()
            {
                job = new SelectList(ServiceRequestImp.GetRequest(), "Id", "RequestName"),
                Team = new SelectList(TeamRepository.GetAll(), "Id", "name"),
                Equip = new SelectList(EquipmentRepository.GetAll(), "Id", "EquipmentName")
            };
            return View(cu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTrack(TrackEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                EquipmentTracking dp = new EquipmentTracking()
                {
                    Equipment = EquipmentRepository.Get(model.EquipmentId),
                    Team = TeamRepository.Get(model.TeamId),
                    ServiceRequest = ServiceRequestImp.Get(model.ServiceRequestId),
                    EquipmentDate = DateTime.Parse(model.EquipmentDate),
                    Status = "In use"
                };
                EquipmentTrackingImp.Add(dp);
                EquipmentTrackingImp.Commit();

                HttpContext.Session.SetString("EquipmentTrackSuccess", "The Equipment has been added successfully");

                return RedirectToAction("CreateTrack");
            }

            TrackEditViewModel cu = new TrackEditViewModel()
            {
                job = new SelectList(ServiceRequestImp.GetRequest(), "Id", "RequestName"),
                Team = new SelectList(TeamRepository.GetAll(), "Id", "name"),
                Equip = new SelectList(EquipmentRepository.GetAll(), "Id", "EquipmentName")
            };
            return View(cu);
        }

        [HttpGet]
        public IActionResult ViewTrackEquipment()
        {
            return View(EquipmentTrackingImp.GetAll());
        }
        

        [HttpGet]
        public IActionResult EditTrackEquipment(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var p = EquipmentTrackingImp.Get(id);

            TrackEditViewModel cu = new TrackEditViewModel()
            {
                EquipmentId=p.Equipment.Id,
                TeamId=p.Team.Id,
                ServiceRequestId=p.ServiceRequest.Id,
                Date=p.EquipmentDate.ToShortDateString(),
                Time=p.EquipmentDate.ToShortTimeString(),
                Status=p.Status,
                job = new SelectList(ServiceRequestImp.GetRequest(), "Id", "RequestName"),
                Team = new SelectList(TeamRepository.GetAll(), "Id", "name"),
                Equip = new SelectList(EquipmentRepository.GetAll(), "Id", "EquipmentName")
            };
            return View(cu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTrackEquipment(TrackEditViewModel model,int id)
        {
            if (ModelState.IsValid)
            {
                EquipmentTracking dp = new EquipmentTracking()
                {
                    Id=id,
                    Equipment = EquipmentRepository.Get(model.EquipmentId),
                    Team =TeamRepository.Get(model.TeamId),
                    ServiceRequest =ServiceRequestImp.Get(model.ServiceRequestId),
                    EquipmentDate = DateTime.Parse(model.EquipmentDate),
                    Status = "Returned"
                };
                EquipmentTrackingImp.Update(dp);
                EquipmentTrackingImp.Commit();

                return RedirectToAction("ViewTrackEquipment");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteEquipmentTrack(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var p = EquipmentTrackingImp.Get(id);
            EquipmentTrackingImp.Delete(p);

            return RedirectToAction("ViewTrackEquipment");
        }
    }
}
