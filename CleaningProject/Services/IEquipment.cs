using CleaningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IEquipment
    {
        IEnumerable<Equipment> GetAll();
        void Add(Equipment value);
        Equipment Get(int? id);
        void update(Equipment value);
        void delete(Equipment value);
        bool Exist(string value);
        void Commit();
    }
}
