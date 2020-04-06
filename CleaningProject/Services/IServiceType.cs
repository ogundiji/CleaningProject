using CleaningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IServiceType
    {
        void Add(ServiceRecord value);
        void Delete(ServiceRecord value);
        void Update(ServiceRecord value);
        IEnumerable<ServiceRecord> GetAll();
        ServiceRecord Get(int? id);
        bool TypeExist(string value);
        void Commit();
    }
}
