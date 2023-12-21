using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.Server.Services;
namespace SignalR.Server.Hubs;

public class ChatHub : Hub
{
    //public async Task SendMessage(string user, string message)
    //{
    //    await Clients.All.SendAsync("ReceiveMessage", user, message);
    //}
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {

        var httpContext = Context.GetHttpContext();
        var userName = httpContext?.Request.Cookies["UserName"];
        var isActive = await _context.login
                .FirstOrDefaultAsync(x => x.UserName == userName);
        if (!isActive.Status)
        {
            isActive.ConnectionId = Context.ConnectionId;
            _context.Update(isActive);
            await _context.SaveChangesAsync();
        }
        Console.WriteLine("Connection Started");
        await base.OnConnectedAsync();
    }
}
