using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace EcommerceMariaDB.Repository
{
    public class EmailRepo : IEmailRepo
    {
        private readonly IConfiguration _configuration;
        public EmailRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(Email request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("EmailSettings:Host").Value,
                         int.Parse(_configuration.GetSection("EmailSettings:Port").Value!),
                         MailKit.Security.SecureSocketOptions.StartTls);

            smtp.Authenticate(_configuration.GetSection("EmailSettings:Username").Value,
                              _configuration.GetSection("EmailSettings:Password").Value);

            smtp.Send(email);
            smtp.Disconnect(true);

            //smtp.Host = "smtp.ethereal.email";
            //smtp.Port = 587;
            //smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = new NetworkCredential("sheridan.ritchie@ethereal.email", "GgYKtNeRhZ4TZn97EJ");
        }
    }
}
