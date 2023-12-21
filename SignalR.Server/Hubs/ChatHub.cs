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
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ChatHub(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task OnConnectedAsync()
    {

        //var httpContext = Context.GetHttpContext();
        //var userName = httpContext?.Request.Cookies["UserName"];
        //var userName = httpContext?.Current.Session.GetString("UserName");
        //var httpContext = _httpContextAccessor.HttpContext;
        //var userName = httpContext?.Request.Cookies["my_cookie"];
        var isActive = await _context.login
                .FirstOrDefaultAsync(x => x.UserName == "mgchit");
        if (isActive is not null)
        {
            isActive.ConnectionId = Context.ConnectionId;
            _context.Update(isActive);
            await _context.SaveChangesAsync();
        }
        else
        {
           
        }
        Console.WriteLine("Connection Started");
        await base.OnConnectedAsync();
    }
}
