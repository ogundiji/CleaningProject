using CleaningProject.Models;
using CleaningProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public class AppointmentImpl:IAppointment
    {
        private CleaningUserDbContext _context;

        public AppointmentImpl(CleaningUserDbContext context)
        {
            _context = context;
        }

        

        public List<AppointViewModel> GetAppointment(string id)
        {
            var p = _context.CleaningItem
                .Include(x => x.ServiceRequest)
                .Include(x => x.Team)
                .Where(x => x.ServiceRequest.Customer.Id.Equals(id))
                .ToList();

            List<AppointViewModel> op = new List<AppointViewModel>();
            foreach(var k in p)
            {
                var pm = new AppointViewModel()
                {
                    CleaningItemId = k.Id,
                    Customer = k.ServiceRequest.Customer.Fullname,
                    Service = k.ServiceRequest.Service.ServiceName,
                    ServiceType = k.ServiceRequest.ServiceType.ServiceType,
                    Staff = getStaff(k.Team.Id),
                    Days = FilterDays(k.ServiceRequest.DaysOfWork),
                    Description = k.Description,
                    Address=k.ServiceRequest.Address,
                    Phone=k.ServiceRequest.Phone,
                    City=k.ServiceRequest.City,
                    ServiceDate = k.ServiceRequest.SheduleDate,
                    Status = k.Status
                };
                op.Add(pm);
            }
            var kol = op.Where(x => x.Staff.Equals(id)).ToList();
            return kol;
        }

        public string getStaff(int Id)
        {
            var po = _context.ConfigureTeam.Include(x => x.Team).Include(x => x.Staff).FirstOrDefault(x => x.Team.Id== Id);
            return po.Staff.Fullname;
        }

       
        public List<DaysOfWeek> GetDays()
        {
            var k = new List<DaysOfWeek>
                {
                    new DaysOfWeek{Id=1,name="Monday",IsChecked=false},
                    new DaysOfWeek{Id=2,name="Tuesday",IsChecked=false},
                    new DaysOfWeek{Id=3,name="Wednesday",IsChecked=false},
                    new DaysOfWeek{Id=4,name="Thursday",IsChecked=false},
                    new DaysOfWeek{Id=5,name="Friday",IsChecked=false},
                    new DaysOfWeek{Id=6,name="Saturday",IsChecked=false},
                    new DaysOfWeek{Id=7,name="Sunday",IsChecked=false}
                };

            return k;

        }

        public List<DaysOfWeek> FilterDays(string model)
        {
            List<string> result = SplitDays(model);
            List<DaysOfWeek> op = GetDays();
            foreach (var p in op)
            {
                if (result.Contains(p.name))
                {
                    p.IsChecked = true;
                }
            }

            return op;
        }
        public List<string> SplitDays(string value)
        {
            List<string> lol = new List<string>();
            var p = value.Split(",");
            foreach (var z in p)
            {
                lol.Add(z);
            }
            return lol;
        }

        public CleaningItem Get(int? id)
        {
            return _context.CleaningItem.Find(id);
        }

        public void Update(CleaningItem po)
        {
            _context.Entry(po).State = EntityState.Modified;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public Company GetCompany()
        {
            Company p = _context.company.FirstOrDefault();

            return p;
        }

        public ServiceRequest GetRequest(int id)
        {
            return _context.ServiceRequest.FirstOrDefault(x => x.Id == id);
        }
    }
}
