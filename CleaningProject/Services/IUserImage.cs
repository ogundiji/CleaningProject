using CleaningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface IUserImage
    {
        void AddUserImage(UserImageRecord value);
        void commit();
        void DeleteUserImage(UserImageRecord value);
    }
}
