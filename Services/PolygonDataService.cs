
using System.Text.Json;
using ASAP_Clients.Entities;
using ASAP_Clients.Manager.IManager;
using ASAP_Clients.PolygonEntities;

namespace ASAP_Clients.Services
{
    public class PolygonDataService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;
        private Timer? _timer = null;

        private IPolygonManager? _polygonManager = null;

        public PolygonDataService(
            IHttpClientFactory httpClientFactory,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RetrieveAndStorePolygonData, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(double.Parse(_configuration["Polygon:CallEvery"])));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
        
        private async void RetrieveAndStorePolygonData(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _polygonManager = scope.ServiceProvider.GetRequiredService<IPolygonManager>();

                await RetrieveAndStorePolygonDataScoped();
            }
        }

        private async Task RetrieveAndStorePolygonDataScoped() 
        {
            if (_polygonManager == null)
                throw new ArgumentNullException();

            var tickers = await _polygonManager.GetPolygonTickers();

            var previousCloses = new List<(long tickerID, PreviousClose previousClose)>();

            foreach (PolygonTicker ticker in tickers)
            {
                var previousClose = await GetPreviousClose(ticker.Name);

                if (previousClose != null)
                {
                    previousCloses.Add((ticker.ID, previousClose));
                }
            }

            if (previousCloses.Any())
            {
                await _polygonManager.SavePreviousCloses(previousCloses);
            }
        }

        private async Task<PreviousClose?> GetPreviousClose(string tickerSymbol)
        {
            string apiKey = _configuration["Polygon:ApiKey"] ?? "";

            var baseUrl = _configuration["Polygon:BaseUrl"];
            var url = $"{baseUrl}{tickerSymbol}/prev?adjusted=true&apiKey={apiKey}";

            var httpClient = _httpClientFactory.CreateClient();

             try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<PreviousClose>(responseJson);
            }
            catch (HttpRequestException)
            {
                //_logger.LogError($"Error retrieving data from Polygon.io: {ex.Message}");
                // Handle the error appropriately (e.g., retry logic, logging)
                return null;
            }
        }
    }
}