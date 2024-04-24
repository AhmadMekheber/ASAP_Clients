using System.Net.Sockets;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace ASAP_Clients.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestMailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult SendMail()
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("ASAP Clients", "asap.clients@yandex.com"));
                message.To.Add(new MailboxAddress("Ahmad Mekheber", "a.c.mad.1994@gmail.com"));
                message.Subject = "Testing ASAP Clients Program";

                // Create the email body (text or HTML)
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "This email is for Testing ASAP Clients Program.";
                // OR (for HTML body)
                // bodyBuilder.HtmlBody = @"<h1>This is the HTML body of your email</h1>";
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // Read SMTP details from configuration (appsettings.json)
                var host = _configuration["SMTP:Host"];
                var port = int.Parse(_configuration["SMTP:Port"]);
                var username = _configuration["SMTP:Username"];
                var password = _configuration["SMTP:Password"];

                client.CheckCertificateRevocation = false;
                // Connect to the server (consider using SSL/TLS for security)
                client.Connect(host, port, true);

                // Optional: Authenticate with username and password (if required)
                client.Authenticate(username, password);

                // Send the email message
                client.Send(message);

                // Disconnect from the server
                client.Disconnect(true);

                Console.WriteLine("Email sent successfully!");
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok();
        }
    }
}