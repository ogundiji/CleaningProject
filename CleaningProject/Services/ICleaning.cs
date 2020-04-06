using CleaningProject.Models;
using CleaningProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface ICleaning
    {
        void Add(CleaningItem value);
        CleaningItem Get(int ?id);
        IEnumerable<ViewCleaningModel> GetAll();
        void Update(CleaningItem value);
        void Delete(CleaningItem value);
        void Commit();
        bool Exist(int value, int value2);

    }
}
