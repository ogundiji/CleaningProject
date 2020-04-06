using CleaningProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private CleaningUserDbContext _db;

        public CompanyRepository(CleaningUserDbContext db)
        {
            _db = db;
        }
        public void Add(Company value)
        {
            _db.company.Add(value);
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public void Delete(Company value)
        {
            _db.company.Remove(value);
        }

        public Company Get(int? id)
        {
            var k = _db.company.Find(id);
            return k;
        }

        public IEnumerable<Company> GetAll()
        {
            return _db.company.ToList();
        }

        public Company GetCompany()
        {
            return _db.company.FirstOrDefault();
        }
     
        public void Update(Company value)
        {
            _db.Entry(value).State = EntityState.Modified;
        }
    }
}
