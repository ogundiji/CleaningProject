using CleaningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IEmailService
    {
        void Send(string EmailTo,string user,string subject,string content);
    }

}
