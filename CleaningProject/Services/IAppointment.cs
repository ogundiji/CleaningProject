using CleaningProject.Models;
using CleaningProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IAppointment
    {
       List<AppointViewModel> GetAppointment(string Id);
       CleaningItem Get(int ?id);
       void Update(CleaningItem po);
       Company GetCompany();
       ServiceRequest GetRequest(int id);
       void Commit();
    }
}
