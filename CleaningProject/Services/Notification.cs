using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public interface Notification
    {
        string Alert(string title, string message, string notifificationType);
    }
}
