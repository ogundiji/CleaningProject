using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.Services;
using CleaningProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleaningProject.Controllers
{
    public class CompanyController : Controller
    {
        private ICompanyRepository CompanyRepository;

        public CompanyController(ICompanyRepository CompanyRepository)
        {
            this.CompanyRepository = CompanyRepository;
        }
        [HttpGet]
        public IActionResult CreateCompany()
        {
            ViewBag.CompanySuccess = HttpContext.Session.GetString("CompanySuccess");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompany(CompanyEditModel values, List<IFormFile> logo)
        {
            if (ModelState.IsValid)
            {
                var comp = CompanyRepository.GetAll().LongCount();
                int len = (int)comp;
                if (len >= 1)
                {
                    //cmpany exist
                    ViewBag.CompanyExist = "You Cannot Create More than one company account";
                }
                else
                {
                    IFormFile f = logo.FirstOrDefault();
                    if (f.Length > 0)
                    {
                        //check if an image is uploaded
                        if (IsImage(f))
                        {
                            byte[] imageData = null;
                            using (var binary = new MemoryStream())
                            {
                                await f.CopyToAsync(binary);
                                imageData = binary.ToArray();
                            }
                            Company k = new Company
                            {
                                name = values.name,
                                email = values.email,
                                PhoneNumber = values.PhoneNumber,
                                Address = values.Address,
                                Logo = imageData,
                                companyCreated = DateTime.Now
                            };
                            CompanyRepository.Add(k);
                            CompanyRepository.Commit();

                            HttpContext.Session.SetString("CompanySuccess", "successfully created company's account");

                            return RedirectToAction("CreateCompany");
                        }
                        else
                        {
                            //not of the right format
                            ViewBag.NotRightFormat = "The image is not in the right format";
                        }

                    }
                    else
                    {
                        //no image is uploaded
                        ViewBag.noUpload = "You have not uploaded any image";
                    }
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult ViewCompany()
        {
            IEnumerable<Company> k = CompanyRepository.GetAll();
            return View(k);
        }

        [HttpGet]
        public IActionResult DeleteCompany(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            var comp = CompanyRepository.Get(id);
            CompanyRepository.Delete(comp);
            CompanyRepository.Commit();

            return RedirectToAction("ViewComany", "Company");
        }

        [HttpGet]
        public IActionResult EditCompany(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("400");
            }
            Company comp = CompanyRepository.Get(id);
            CompanyEditModel po = new CompanyEditModel()
            {
                name = comp.name,
                email = comp.email,
                PhoneNumber = comp.PhoneNumber,
                Address = comp.Address,
                logo = comp.Logo,
                Date = comp.companyCreated.ToShortDateString(),
                Time = comp.companyCreated.ToShortTimeString()
            };
            return View(po);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompany(CompanyEditModel values, int id, List<IFormFile> logo)
        {
            if (ModelState.IsValid)
            {
                IFormFile f = logo.FirstOrDefault();
                if (f.Length > 0)
                {
                    if (IsImage(f))
                    {
                        byte[] imageData = null;
                        using (var binary = new MemoryStream())
                        {
                            await f.CopyToAsync(binary);
                            imageData = binary.ToArray();
                        }
                        var kp = new Company
                        {
                            Id=id,
                            name = values.name,
                            email = values.email,
                            PhoneNumber = values.PhoneNumber,
                            Address = values.Address,
                            companyCreated = DateTime.Parse(values.companyCreated),
                            Logo = imageData
                        };
                        CompanyRepository.Update(kp);
                        CompanyRepository.Commit();

                        return RedirectToAction("ViewCompany");
                    }
                }
            }
            return View();
        }


        public bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            foreach (var item in formats)
            {
                if (file.FileName.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}