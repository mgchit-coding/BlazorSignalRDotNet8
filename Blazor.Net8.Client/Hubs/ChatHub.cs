using Blazor.Net8.Client.Models;
using Blazor.Net8.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor.Extensions;
using System.Runtime.CompilerServices;
using SignalR.Server.Models;
using static MudBlazor.CategoryTypes;

namespace Blazor.Net8.Client.Hubs
{
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
            //var customAuthStateProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            //var model = await customAuthStateProvider.GetUserData();
            //var result = _context.login.FirstOrDefault(x => x.UserName == model.UserName);
            //if (result != null)
            //{
            //    var notification = _context.notification.FirstOrDefault(x => x.LoginId == result.Id);
            //    if (notification != null)
            //    {
            //        notification.ConnectionId = Context.ConnectionId;
            //        _context.notification.Update(notification);
            //        await _context.SaveChangesAsync();
            //    }
            //}
        }

        // public async Task CreateUserConnection(LoginModel requestModel)
        // {
        //     var result = await _context.Notification.FirstOrDefaultAsync(x => x.LoginId == requestModel.Id);
        //     var model = new NotificationDataModel
        //     {
        //         ConnectionId = Context.ConnectionId,
        //         // LoginId = requestModel.Id,
        //     };
        //     if (result is null)
        //     {
        //         _context.Notification.Add(model);
        //         await _context.SaveChangesAsync();
        //     }
        //     else
        //     {
        //         //_context.Entry(model).State = EntityState.Modified;
        //         _context.Notification.Update(model);
        //         await _context.SaveChangesAsync();
        //     }
        //     await Groups.AddToGroupAsync(Context.ConnectionId, requestModel.UserName);
        // }

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

        public async Task PushNotification(LoginDataModel model)
        {
            var item = await _context.Login
                .FirstOrDefaultAsync(x => x.UserId == model.UserId &&
                x.SessionId != model.SessionId);
            if (item is not null && item.ConnectionId is not null)
                await Clients.Client(item.ConnectionId).SendAsync("GoToLogin");
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
}
