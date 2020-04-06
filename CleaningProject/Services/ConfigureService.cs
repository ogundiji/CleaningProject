using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningProject.Services
{
    public class ConfigureService : IServiceConfig
    {
        private CleaningUserDbContext context;

        public ConfigureService(CleaningUserDbContext context)
        {
            this.context = context;
        }
        public void Add(ServiceTypeRecord value)
        {
            context.ServiceTypeRecord.Add(value);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(ServiceTypeRecord value)
        {
            context.ServiceTypeRecord.Remove(value);
        }

        public ServiceTypeRecord Get(int? id)
        {
            return context.ServiceTypeRecord.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ServiceTypeRecord> GetAll()
        {

            var k = context.ServiceTypeRecord.Include(x => x.po).Include(x => x.qo).ToList();
            return k;
        }

        public ServiceTypeRecord GetServiceType(int service, int ServiceType)
        {
           var p = context.ServiceTypeRecord.FirstOrDefault(x => x.ServiceId == service && x.ServiceTypeId == ServiceType);
           return p;   
        }

        public int MaxValue()
        {
            var k = GetAll();
            int z = k.Count();
            int kp;
            if (z == 0)
            {
                kp = 1;
            }
            else
            {
                kp = k.Max(x => x.Id);
            }
           
            return kp;
        }

        public bool TypeExist(int value,int value2)
        {
            return context.ServiceTypeRecord.Any(x => x.ServiceId == value && x.ServiceTypeId == value2);
        }

        public void Update(ServiceTypeRecord value)
        {
            context.Entry(value).State = EntityState.Modified;
        }
    }
}
