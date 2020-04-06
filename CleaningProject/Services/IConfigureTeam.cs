using CleaningProject.Models;
using CleaningProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IConfigureTeam
    {
        void Add(ConfigureTeam value);
        void Update(ConfigureTeam value);
        void Delete(ConfigureTeam value);
        IEnumerable<ViewConfigureEditModel> GetAll();
        ConfigureTeam Get(int? id);
        void Commit();
    }
}
