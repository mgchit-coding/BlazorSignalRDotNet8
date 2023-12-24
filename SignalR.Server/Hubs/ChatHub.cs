﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using SignalR.Server.Models;
using SignalR.Server.Services;
using System.Runtime.CompilerServices;

namespace SignalR.Server.Hubs
{
    public class ChatHub 
    {
        private HubConnection _connection;
        private readonly AppDbContext _appDbContext;
        public ChatHub(AppDbContext appDbContext)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5261/chathub")
                .Build();
            _appDbContext = appDbContext;
        }

        // public async Task SendNotification(LoginModel model)
        // {
        //     if (_connection.State == HubConnectionState.Disconnected)
        //         await _connection.StartAsync();
        //     var item = await _appDbContext.Notification
        //         .FirstOrDefaultAsync(x => x.LoginId == model.Id);
        //     //await _connection.InvokeAsync("PushNotification", item.ConnectionId);
        //     await _connection.InvokeAsync("PushNotification", model.UserName);
        // }
        //
        public async Task PushNotification(LoginDataModel model)
        {
            if (_connection.State == HubConnectionState.Disconnected)
                await _connection.StartAsync();
            await _connection.InvokeAsync("PushNotification", model);
        }
    }
}
