using CleaningProject.Models;
using CleaningProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface ITeam
    {
        void Add(Team value);
        void Update(Team value);
        void Delete(Team value);
        Team Get(int? id);
        void Commit();
        bool TeamExist(string teamName);
        IEnumerable<Team> GetAll();

    }
}
