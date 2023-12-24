using Blazor.Net8.Client.Models;
using Blazor.Net8.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor.Extensions;
using System.Runtime.CompilerServices;
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

        public async Task CreateUserConnection(LoginModel requestModel)
        {
            var result = await _context.notification.FirstOrDefaultAsync(x => x.LoginId == requestModel.Id);
            var model = new NotificationDataModel
            {
                ConnectionId = Context.ConnectionId,
                LoginId = requestModel.Id,
            };
            if (result is null)
            {
                _context.notification.Add(model);
                await _context.SaveChangesAsync();
            }
            else
            {
                //_context.Entry(model).State = EntityState.Modified;
                _context.notification.Update(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task PushNotification(string connectionId)
        {
            var id = connectionId;
            await Clients.All.SendAsync("GoToLogin", connectionId);
        }

        public async Task Logout(string connectioinId)
        {
            await Clients.Clients(connectioinId).SendAsync("GoToLogout");
        }
    }
}
