using CleaningProject.Models;
using CleaningProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IRequestService
    {
        void Add(ServiceRequest value);
        void Update(ServiceRequest value);
        void Delete(ServiceRequest value);
        ServiceRequest Get(int ?Id);
        IEnumerable<ServiceRequest> GetAll(string Id);
        IEnumerable<ViewServiceRequestModel> GetService();
        IEnumerable<ServiceRequest> GetRequest();
        void Commit();
       
    }
}
