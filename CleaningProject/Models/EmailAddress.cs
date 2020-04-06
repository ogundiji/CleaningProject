using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningProject.Models
{
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class EmailMessage
    {
        public EmailAddress ToAddresses;
        public EmailAddress FromAddresses;

        public EmailMessage()
        {
            ToAddresses = new EmailAddress();
            FromAddresses = new EmailAddress();
        }
        public string Subject { get; set; }
        public string Content { get; set; }
    }

    public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
    }

    public class EmailConfiguration : IEmailConfiguration
    {
        public string SmtpServer { get; set; }
	    public int SmtpPort  { get; set; }
	    public string SmtpUsername { get; set; }
	    public string SmtpPassword { get; set; }
    }
}
