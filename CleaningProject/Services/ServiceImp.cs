using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;

namespace CleaningProject.Services
{
    public class ServiceImp : IService
    {
        private CleaningUserDbContext context;

        public ServiceImp(CleaningUserDbContext context)
        {
            this.context = context;
        }
        public void Add(Service value)
        {
            context.Service.Add(value);
        }

        public void Delete(Service value)
        {
            context.Service.Remove(value);
        }

        public Service Get(int ?id)
        {
            return context.Service.Find(id);
        }

        public IEnumerable<Service> GetAll()
        {

            return context.Service.ToList();
        }

        public bool TypeExist(string value)
        {
            return context.Service.Any(x => x.ServiceName.ToUpper().Equals(value.ToUpper()));
        }

        public void Update(Service value)
        {
            //context.Update(value);
            context.Entry(value).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void Commit()
        {
            context.SaveChanges();
        }

        public string GetServiceName(int id)
        {
            var c = context.Service.Find(id);
            return c.ServiceName;
        }

        
    }
}
