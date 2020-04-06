using System.Collections.Generic;
using System.Linq;
using CleaningProject.Models;
using CleaningProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleaningProject.Services
{
    public class ConfigureTeamRepository : IConfigureTeam
    {
        private CleaningUserDbContext _context { get; set; }
        public ConfigureTeamRepository(CleaningUserDbContext context)
        {
            _context = context;
        }
        public void Add(ConfigureTeam value)
        {
            _context.ConfigureTeam.Add(value);
        }

        public void Delete(ConfigureTeam value)
        {
            _context.ConfigureTeam.Remove(value);
        }

        public ConfigureTeam Get(int? id)
        {
            return _context.ConfigureTeam.Find(id);
        }

        public IEnumerable<ViewConfigureEditModel> GetAll()
        {
            var kpo = _context.ConfigureTeam.Include(x => x.Team).Include(x => x.Staff).ToList();
            List<ViewConfigureEditModel> polly = new List<ViewConfigureEditModel>();
            foreach(var y in kpo)
            {
                var pq = new ViewConfigureEditModel()
                {
                    Id=y.Id,
                    Team=y.Team.name,
                    Staff=y.Staff.Fullname
                };
                polly.Add(pq);
            }

            return polly;
           
        }

        public void Update(ConfigureTeam value)
        {
            _context.Entry(value).State = EntityState.Modified;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

    }
}
