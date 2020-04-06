using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleaningProject.Services
{
    public class TeamRepository:ITeam
    {
        private CleaningUserDbContext context;

        public TeamRepository(CleaningUserDbContext context)
        {
            this.context = context;
        }
        public void Add(Team value)
        {
            context.team.Add(value);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(Team value)
        {
            context.team.Remove(value);
        }

        public Team Get(int? id)
        {
            return context.team.Find(id);
        }

        
        public IEnumerable<Team> GetAll()
        {
            return context.team.ToList();
        }

        public bool TeamExist(string teamName)
        {
            return context.team.Any(x => x.name.ToUpper().Equals(teamName.ToUpper()));
        }

        public void Update(Team value)
        {
            context.Entry(value).State = EntityState.Modified;
        }

    }
}
