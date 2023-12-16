using Microsoft.AspNetCore.SignalR;
namespace SignalR.Server.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("Connection Started");
    }
}
