using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Services
{
    public class NotificationImpl : Notification
    {
        public string Alert(string title, string message, string notifificationType)
        {
            var msg = "swal('" + title + "','" + message + "','" + notifificationType + "')" + "";
            return msg;
        }
    }
}
