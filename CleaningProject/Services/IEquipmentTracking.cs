using CleaningProject.Models;
using CleaningProject.ViewModels;
using System.Collections.Generic;

namespace CleaningProject.Services
{
    public interface IEquipmentTracking
    {
        void Add(EquipmentTracking model);
        void Update(EquipmentTracking model);
        EquipmentTracking  Get(int? id);
        void Delete(EquipmentTracking model);
        IEnumerable<ViewUsedEquipment> GetAll();
        void Commit();
    }
}
