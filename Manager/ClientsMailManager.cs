using System.Net.Sockets;
using ASAP_Clients.Entities;
using ASAP_Clients.Manager.IManager;
using ASAP_Clients.Repository.IRepository;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace ASAP_Clients.Manager
{
    public class ClientsMailManager : IClientsMailManager
    {
        private readonly IClientsUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        private SmtpClient? _smtpClient;

        public ClientsMailManager(IClientsUnitOfWork unitOfWork,
                                  IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        private MailboxAddress SenderMailboxAddress => new MailboxAddress("ASAP Clients", "asap.clients@yandex.com");

        private async Task ConnectSmtpClientIfRequired() 
        {
            if (_smtpClient != null && _smtpClient.IsConnected)
                return;

            _smtpClient = new SmtpClient();

            // Read SMTP details from configuration (appsettings.json)
            var host = _configuration["SMTP:Host"];
            var port = int.Parse(_configuration["SMTP:Port"]);
            var username = _configuration["SMTP:Username"];
            var password = _configuration["SMTP:Password"];

            // Connect to the server (consider using SSL/TLS for security)
            await _smtpClient.ConnectAsync(host, port, true);

            // Optional: Authenticate with username and password (if required)
            await _smtpClient.AuthenticateAsync(username, password);
        }

        private async Task DisconnectSmtpClient()
        {
            if (_smtpClient != null && _smtpClient.IsConnected)
            {
                // Disconnect from the server
                await _smtpClient.DisconnectAsync(true);

                _smtpClient.Dispose();

                _smtpClient = null;
            }
        }

        public async Task SendMailsToUnnotifiedClients()
        {
            var previousCloseResponses = await _unitOfWork.PreviousCloseResponseRepository.GetResponsesToNotify().ToListAsync();
            var clients = await _unitOfWork.ClientRepository.GetNotDeleted().ToListAsync();
            
            foreach (var previousCloseResponse in previousCloseResponses)
            {
                await SendPreviousCloseResponse(previousCloseResponse, clients);
            }

            await DisconnectSmtpClient();

            previousCloseResponses.ForEach(previousCloseResponse => previousCloseResponse.IsClientsNotified = true);

            if (previousCloseResponses.Any())
            {
                await _unitOfWork.BulkSaveChangesAsync();
            }
        }

        private async Task SendPreviousCloseResponse(PreviousCloseResponse previousCloseResponse, List<Client> clients)
        {
            foreach(Client client in clients)
            {
                await SendPreviousCloseResponseToClient(previousCloseResponse, client);
            }
        }

        private async Task SendPreviousCloseResponseToClient(PreviousCloseResponse previousCloseResponse, Client client)
        {
            PolygonRequest polygonRequest = previousCloseResponse.Request;
            PolygonTicker polygonTicker = polygonRequest.Ticker;

            try
            {
                var message = new MimeMessage();

                message.From.Add(SenderMailboxAddress);
                message.To.Add(new MailboxAddress("Ahmad Mekheber", client.Email));
                message.Subject = $"{polygonTicker.Name} ({polygonTicker.CompanyName}) Previouse Close";

                // Create the email body (text or HTML)
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "This email is for Testing ASAP Clients Program.";
                // OR (for HTML body)
                // bodyBuilder.HtmlBody = @"<h1>This is the HTML body of your email</h1>";
                message.Body = bodyBuilder.ToMessageBody();

                await ConnectSmtpClientIfRequired();
                // Send the email message
                await _smtpClient.SendAsync(message);

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
        }
    }
}