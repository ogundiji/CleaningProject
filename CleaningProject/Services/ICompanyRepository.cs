using CleaningProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAll();
        void Add(Company value);
        Company Get(int? id);
        void Update(Company value);
        void Delete(Company value);
        Company GetCompany();
        void Commit();
      }
}