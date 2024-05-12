using Microsoft.AspNetCore.SignalR;
using System.Net.Http;

namespace TaskWebApp.Services.ApiServices
{
    public class NewsService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IHubContext<NewsHub> _hubContext;
        private readonly HttpClient _httpClient;
        public const string API_KEY = "NT0u2YEciB35Uht6qfskOYVNB1wMk0rwahI3Ockf";
        public const int INTERVAL = 60;

        public NewsService(IHubContext<NewsHub> hubContext, HttpClient httpClient)
        {
            _hubContext = hubContext;
            _httpClient = httpClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendTopNews, null, TimeSpan.Zero, TimeSpan.FromSeconds(INTERVAL));
            return Task.CompletedTask;
        }

        private async void SendTopNews(object state)
        {
            try
            {
                string newsData = await _httpClient.GetStringAsync($"https://api.thenewsapi.com/v1/news/top?api_token={API_KEY}");

                await _hubContext.Clients.All.SendAsync("ReceiveNews", newsData);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error sending news: {ex.Message}");
            }
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
    }
}
