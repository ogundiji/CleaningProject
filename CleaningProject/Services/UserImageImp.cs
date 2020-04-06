using CleaningProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public class UserImageImp : IUserImage
    {
        private CleaningUserDbContext context;

        public UserImageImp(CleaningUserDbContext context)
        {
            this.context = context;
        }
        public void AddUserImage(UserImageRecord values)
        {
            context.UserImg.Add(values);
        }

        public void commit()
        {
            context.SaveChanges();
        }

        public void DeleteUserImage(UserImageRecord value)
        {
            context.UserImg.Remove(value);
        }
    }
}
