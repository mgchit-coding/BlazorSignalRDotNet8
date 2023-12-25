using Microsoft.AspNetCore.SignalR;

namespace BlazorDotNet8.Client.Hubs;

public class ChatHub : Hub
{
    private readonly AppDbContext _context;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public ChatHub(AppDbContext context, AuthenticationStateProvider authenticationStateProvider)
    {
        _context = context;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public override async Task OnConnectedAsync()
    {
        ConnectedUser.Ids.Add(Context.ConnectionId);
        Console.WriteLine($"First Connection Id {Context.ConnectionId}");
    }

    public async Task CreateUserConnection(LoginDataModel model, string userType)
    {
        var item = await _context.Login
            .FirstOrDefaultAsync(x => x.UserId == model.UserId &&
                                      x.SessionId == model.SessionId);
        if (item is not null)
        {
            item.ConnectionId = Context.ConnectionId;
            _context.Login.Update(item);
            await _context.SaveChangesAsync();
            await Groups.AddToGroupAsync(Context.ConnectionId, userType);
        }
    }

    public async Task PushNotification(List<string> connectionIds)
    {
        //await Clients.Clients(connectionIds).SendAsync("GoToLogin");
        foreach (var connectionId in connectionIds)
        {
            await Clients.Client(connectionId).SendAsync("GoToLogin");
        }
    }

    public async Task Logout(string connectionId)
    {
        await Clients.Users(connectionId).SendAsync("GoToLogout", connectionId);
    }
}
public static class ConnectedUser
{
    public static List<string> Ids = new List<string>();
}

public class ConnectionIdStateContainer
{
    private LoginDataModel? data;

    public LoginDataModel Data
    {
        get => data!;
        set
        {
            data = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}