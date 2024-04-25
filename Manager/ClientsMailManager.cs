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

        private MailboxAddress SenderMailboxAddress => new MailboxAddress(_configuration["Mail:SenderName"], _configuration["Mail:SenderMailAddress"]);

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

            if (previousCloseResponses.Any() == false)
                return;

            var clients = await _unitOfWork.ClientRepository.GetNotDeleted().ToListAsync();
            
            await SendPreviousCloseResponse(previousCloseResponses, clients);

            await DisconnectSmtpClient();

            previousCloseResponses.ForEach(previousCloseResponse => previousCloseResponse.IsClientsNotified = true);

            if (previousCloseResponses.Any())
            {
                await _unitOfWork.BulkSaveChangesAsync();
            }
        }

        private async Task SendPreviousCloseResponse(List<PreviousCloseResponse> previousCloseResponses, List<Client> clients)
        {
            foreach(Client client in clients)
            {
                await SendPreviousCloseResponseToClient(previousCloseResponses, client);
            }
        }

        private async Task SendPreviousCloseResponseToClient(List<PreviousCloseResponse> previousCloseResponses, Client client)
        {
            try
            {
                string htmlBody = string.Empty;
                
                foreach (PreviousCloseResponse previousCloseResponse in previousCloseResponses) 
                {
                    htmlBody += generatepriceTable(previousCloseResponse);
                }

                var message = new MimeMessage();

                message.From.Add(SenderMailboxAddress);
                message.To.Add(new MailboxAddress($"{client.FirstName} {client.LastName}", client.Email));
                message.Subject = _configuration["Mail:Subject"];

                // Create the email body (text or HTML)
                var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };

                message.Body = bodyBuilder.ToMessageBody();

                await ConnectSmtpClientIfRequired();
                // Send the email message
                await _smtpClient.SendAsync(message);

                //Wait 2 seconds after sending each mail because the mail provider would consider this a spam email and it won't be delivered.
                Thread.Sleep(2000);

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

        private string generatepriceTable(PreviousCloseResponse previousCloseResponse) {
            return pricesTableTemplate
                        .Replace("@TickerSymbol", previousCloseResponse.Request?.Ticker?.Name)
                        .Replace("@TickerCompany", previousCloseResponse.Request?.Ticker?.CompanyName)
                        .Replace("@OpenPrice", previousCloseResponse.OpenPrice.ToString())
                        .Replace("@ClosePrice", previousCloseResponse.ClosePrice.ToString())
                        .Replace("@LowestPrice", previousCloseResponse.LowestPrice.ToString())
                        .Replace("@HighestPrice", previousCloseResponse.HighestPrice.ToString())
                        .Replace("@VolumeWeightedAveragePrice", previousCloseResponse.VolumeWeightedAveragePrice.ToString())
                        .Replace("@TransactionsCount", previousCloseResponse.TransactionsCount.ToString())
                        .Replace("@TradingVolume", previousCloseResponse.TradingVolume.ToString())
                        .Replace("@AggregateWindowDate", previousCloseResponse.AggregateWindowDate.ToString());
        }

        private string pricesTableTemplate = @"
            <h3>@TickerSymbol (@TickerCompany)</h3>
            <table style='border: 1px solid #ddd; border-collapse: collapse;'>
                <tr>
                    <th style='padding: 5px 10px;'>Open</th>
                    <th style='padding: 5px 10px;'>Close</th>
                    <th style='padding: 5px 10px;'>Lowest</th>
                    <th style='padding: 5px 10px;'>Highest</th>
                    <th style='padding: 5px 10px;'>Average Price</th>
                    <th style='padding: 5px 10px;'>Transactions Count</th>
                    <th style='padding: 5px 10px;'>Volume</th>
                    <th style='padding: 5px 10px;'>Aggregate Window Date</th>
                </tr>
                <tr>
                    <td style='padding: 5px 10px;'>@OpenPrice</td>
                    <td style='padding: 5px 10px;'>@ClosePrice</td>
                    <td style='padding: 5px 10px;'>@LowestPrice</td>
                    <td style='padding: 5px 10px;'>@HighestPrice</td>
                    <td style='padding: 5px 10px;'>@VolumeWeightedAveragePrice</td>
                    <td style='padding: 5px 10px;'>@TransactionsCount</td>
                    <td style='padding: 5px 10px;'>@TradingVolume</td>
                    <td style='padding: 5px 10px;'>@AggregateWindowDate</td>
                </tr>
            </table>
        ";
    }
}