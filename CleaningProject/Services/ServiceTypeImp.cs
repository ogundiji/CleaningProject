using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;

namespace CleaningProject.Services
{
    public class ServiceTypeImp : IServiceType
    {
        private CleaningUserDbContext context;

        public ServiceTypeImp(CleaningUserDbContext context)
        {
            this.context = context;
        }
        public void Add(ServiceRecord value)
        {
            context.ServiceType.Add(value);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(ServiceRecord value)
        {
            context.ServiceType.Remove(value);
        }

        public ServiceRecord Get(int? id)
        {
            return context.ServiceType.Find(id);
        }

        public IEnumerable<ServiceRecord> GetAll()
        {
            return context.ServiceType.ToList();
        }

        public bool TypeExist(string value)
        {
            return context.ServiceType.Any(x => x.ServiceType.ToUpper().Equals(value.ToUpper()));
        }

        public void Update(ServiceRecord value)
        {
            context.Entry(value).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
