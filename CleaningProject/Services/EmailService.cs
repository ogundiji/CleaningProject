using CleaningProject.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleaningProject.Services
{ 
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private ICompanyRepository CompanyRepository;

        public EmailService(IEmailConfiguration emailConfiguration,ICompanyRepository CompanyRepository)
        {
            _emailConfiguration = emailConfiguration;
            this.CompanyRepository = CompanyRepository;
        }
        public void Send(string EmailTo, string user, string subject, string content)
        {
            var polly = CompanyRepository.GetCompany();
          
            var f = new EmailAddress()
            {
                Name = polly.name,
                Address = polly.email
            };
            var T = new EmailAddress()
            {
                Name = user,
                Address = EmailTo
            };

            EmailMessage cv = new EmailMessage()
            {
                FromAddresses = f,
                ToAddresses = T,
                Content = content,
                Subject = subject
            };

            MimeMessage message = new MimeMessage();

            string a = cv.FromAddresses.Name.ToString();
            string b = cv.FromAddresses.Address.ToString();
            MailboxAddress c = new MailboxAddress(a,b);
            message.From.Add(c);
            message.To.Add(new MailboxAddress(cv.ToAddresses.Name,cv.ToAddresses.Address));
           

            message.Subject = subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
               
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }
    }
    
}
