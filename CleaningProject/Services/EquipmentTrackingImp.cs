using System;
using System.Collections.Generic;
using System.Linq;
using CleaningProject.Models;
using CleaningProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleaningProject.Services
{
    public class EquipmentTrackingImp:IEquipmentTracking
    {
        private CleaningUserDbContext context;

        public EquipmentTrackingImp(CleaningUserDbContext context)
        {
            this.context = context;
        }
        public void Add(EquipmentTracking model)
        {
            context.EquipmentTracking.Add(model);
        }
        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(EquipmentTracking model)
        {
            context.EquipmentTracking.Remove(model);
        }

        public EquipmentTracking Get(int? id)
        {
            return context.EquipmentTracking.Find(id);
        }

        public IEnumerable<ViewUsedEquipment> GetAll()
        {
            List<ViewUsedEquipment> kol = new List<ViewUsedEquipment>();
            List<EquipmentTracking> pol = context.EquipmentTracking
                .Include(x => x.ServiceRequest)
                .Include(x => x.Team)
                .Include(x => x.Equipment)
                .ToList();

             foreach(var k in pol)
             {
                var p = new ViewUsedEquipment()
                {
                    Id=k.Id,
                    Request=k.ServiceRequest.RequestName,
                    Equipment=k.Equipment.EquipmentName,
                    Team=k.Team.name,
                    Status=k.Status,
                    Date=k.EquipmentDate
                };
                kol.Add(p);
             }

            return kol;
           }
        public void Update(EquipmentTracking model)
        {
            context.Entry(model).State = EntityState.Modified;
        }

        public string GetTeam(int id)
        {
            var k = context.team.Find(id);
            return k.name;
        }
       
    }
}
