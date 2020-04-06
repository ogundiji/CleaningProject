using CleaningProject.Models;
using System.Collections.Generic;

namespace CleaningProject.Services
{
    public interface IServiceConfig
    {
        void Add(ServiceTypeRecord value);
        void Delete(ServiceTypeRecord value);
        void Update(ServiceTypeRecord value);
        IEnumerable<ServiceTypeRecord> GetAll();
        ServiceTypeRecord Get(int ?id);
        ServiceTypeRecord GetServiceType(int id, int id2);
        bool TypeExist(int value,int value2);
        int MaxValue();
        void Commit();
    }
}
