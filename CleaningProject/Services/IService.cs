using CleaningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IService
    {
        void Add(Service value);
        void Delete(Service value);
        void Update(Service value);
        IEnumerable<Service> GetAll();
        Service Get(int ?id);
        string GetServiceName(int id);
        bool TypeExist(string value);
        void Commit();
    }
}
