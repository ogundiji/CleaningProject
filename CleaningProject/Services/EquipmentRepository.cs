using CleaningProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public class EquipmentRepository:IEquipment
    {
        private CleaningUserDbContext _db;

        public EquipmentRepository(CleaningUserDbContext context)
        {
            _db = context;
        }

        public void Add(Equipment value)
        {
            _db.equipment.Add(value);
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public void delete(Equipment value)
        {
            _db.equipment.Remove(value);
        }

        public bool Exist(string value)
        {
           return  _db.equipment.Any(x => x.EquipmentName.ToUpper().Equals(value.ToUpper()));
        }

        public Equipment Get(int? id)
        {
            var kp = _db.equipment.Find(id);
            return kp;
        }

        public IEnumerable<Equipment> GetAll()
        {
                return _db.equipment.ToList();
        }

        public void update(Equipment value)
        {
            _db.Entry(value).State = EntityState.Modified;
        }
    }
}
