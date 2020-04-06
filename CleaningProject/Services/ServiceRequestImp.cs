using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleaningProject.Services
{
    
    public class ServiceRequestImp : IRequestService
    {
        private CleaningUserDbContext context;

        public ServiceRequestImp(CleaningUserDbContext context)
        {
            this.context = context;
        }
        public void Add(ServiceRequest value)
        {
            context.ServiceRequest.Add(value);
        }

        public void Update(ServiceRequest value)
        {
            context.Entry(value).State = EntityState.Modified;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public IEnumerable<ServiceRequest> GetRequest()
        {
            return context.ServiceRequest.ToList();
        }

        public ServiceRequest Get(int ?Id)
        {
            return context.ServiceRequest
                .Include(x=>x.Service)
                .Include(x=>x.ServiceType)
                .Include(x=>x.Customer)
                .FirstOrDefault(x => x.Id == Id);
        }
        public IEnumerable<ServiceRequest> GetAll(string Id)
        {
            var k = context.ServiceRequest.Include(x => x.Customer).Include(x => x.Service).Include(x => x.ServiceType).Where(x=>x.Customer.Id.Equals(Id));
            var y= k.ToList();
            return y;
        }

        public IEnumerable<ViewServiceRequestModel> GetService()
        {

            var k = context.ServiceRequest
                .Include(x => x.Service)
                .Include(x => x.ServiceType)
                .Include(x => x.Customer)
                .ToList();

            List<ViewServiceRequestModel> polly = new List<ViewServiceRequestModel>();
            foreach(var z in k)
            {
                ViewServiceRequestModel cu = new ViewServiceRequestModel()
                {
                    Id = z.Id,
                    Customer = z.Customer.Fullname,
                    Service = z.Service.ServiceName,
                    ServiceType = z.ServiceType.ServiceType,
                    SheduleDate=z.SheduleDate,
                    Address = z.Address,
                    Phone = z.Phone,
                    City = z.City,
                    Status = z.Status
                };
                polly.Add(cu);
            }
            var pq = from s in polly
                     select s;
            return pq;
        }
        
        public void Delete(ServiceRequest value)
        {
            context.ServiceRequest.Remove(value);
        }

        public string getUser(string id)
        {
           var k= context.Users.Find(id);
            return k.Fullname;
        }

    }
}