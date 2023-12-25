using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SignalR.Server.Hubs;
using BlazorDotNet8.Models;
using SignalR.Server.Services;
using System.Runtime.CompilerServices;

namespace SignalR.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ChatHub _chatHub;

    public LoginController(AppDbContext context, ChatHub chatHub)
    {
        _context = context;
        _chatHub = chatHub;
    }

    [HttpPost, Route("Login")]
    public async Task<IActionResult> Login(LoginRequestModel requestModel)
    {
        var responseModel = new ResponseModel();
        var item = await _context.User
            .FirstOrDefaultAsync(x => x.UserName == requestModel.UserName &&
                                      x.Password == requestModel.Password);

        LoginDataModel model = new LoginDataModel();
        if (item is null)
        {
            responseModel = new ResponseModel()
            {
                ResponseMessage = "Fail"
            };
            goto result;
        }

        #region Login

        model = new LoginDataModel
        {
            UserId = item.GeneratedUserId,
            SessionId = Guid.NewGuid().ToString()
        };
        await _context.Login.AddAsync(model);
        await _context.SaveChangesAsync();

        #endregion

        #region Logout All UserId (not included current Session Id)

        var loginCount = await _context.Login
            .CountAsync(
                x =>
                    x.UserId == item.GeneratedUserId &&
                    x.SessionId != model.SessionId);
        if (loginCount > 0)
        {
            var connectionIds = await _context.Login.Where(
                    x =>
                        x.UserId == item.GeneratedUserId &&
                        x.SessionId != model.SessionId)
                .Select(x => x.ConnectionId)
                .ToListAsync();
            await _chatHub.PushNotification(connectionIds.Where(x=> x != null).ToList());
        }

        #endregion

        responseModel = new ResponseModel()
        {
            ResponseData = JsonConvert.SerializeObject(model),
            ResponseMessage = item is not null ? "Success" : "Fail"
        };
        result:
        return Ok(responseModel);
    }
}

public class LoginRequestModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
}