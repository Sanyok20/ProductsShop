using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Products.Services
{
    public class EmailService : IEmailSender
    {
        private readonly string _email;
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            string host = "smtp.gmail.com";
            int port = 587;
            string password = "";
            _email = "";

            _smtpClient = new SmtpClient(host, port);
            _smtpClient.Credentials = new NetworkCredential(_email, password);
            _smtpClient.EnableSsl = true;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_email);
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message);
        }
    }
}