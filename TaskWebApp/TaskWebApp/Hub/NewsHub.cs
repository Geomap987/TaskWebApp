using Microsoft.AspNetCore.SignalR;

public class NewsHub : Hub
{
    public async Task SendNews(string news)
    {
        await Clients.All.SendAsync("ReceiveNews", news);
    }
}