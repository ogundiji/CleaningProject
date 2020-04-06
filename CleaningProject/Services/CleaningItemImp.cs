using CleaningProject.Models;
using CleaningProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public class CleaningItemImp:ICleaning
    {
        private CleaningUserDbContext _context;

        public CleaningItemImp(CleaningUserDbContext context)
        {
            _context = context;
        }

        public void Add(CleaningItem value)
        {
            _context.CleaningItem.Add(value);
        }

        public void Delete(CleaningItem value)
        {
            _context.CleaningItem.Remove(value);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public CleaningItem Get(int ?id)
        {
           return _context.CleaningItem.Find(id);
        }

        public IEnumerable<ViewCleaningModel> GetAll()
        {
            var k = _context.CleaningItem
                .Include(x => x.ServiceRequest)
                .Include(x => x.Team)
                .Include(x=>x.ServiceRequest.Customer)
                .Include(x=>x.ServiceRequest.Service)
                .Include(x=>x.ServiceRequest.ServiceType)
                .ToList();

            List<ViewCleaningModel> result = new List<ViewCleaningModel>();
            foreach(var pk in k)
            {
                 var q = new ViewCleaningModel()
                 {
                     Id=pk.Id,
                     Customer=pk.ServiceRequest.Customer.Fullname,
                     Service=pk.ServiceRequest.Service.ServiceName,
                     ServiceType=pk.ServiceRequest.ServiceType.ServiceType,
                     Phone=pk.ServiceRequest.Phone,
                     Address=pk.ServiceRequest.Address,
                     City=pk.ServiceRequest.City,
                     SheduleDate=pk.ServiceRequest.SheduleDate,
                     TeamName=pk.Team.name,
                     Staff=GetTeam(pk.Team)
                 };

                result.Add(q);
            }

            return result;
           
        }

        public void Update(CleaningItem value)
        {
            _context.Entry(value).State = EntityState.Modified;
        }
        public bool Exist(int value,int value2)
        {
            return _context.CleaningItem.Any(x => x.Team.Id == value && x.ServiceRequest.Id == value2);
        }

      
        public List<string> GetTeam(Team TeamID)
        {
            var k = _context.ConfigureTeam
                .Include(x => x.Staff)
                .Include(x => x.Team)
                .Where(x => x.Team.Id == TeamID.Id)
                .ToList();


            List<string> polly = new List<string>();
          
            foreach(var j in k)
            {
               polly.Add(j.Staff.Fullname);
            }

            
            return polly;
        }
       
    }
}
