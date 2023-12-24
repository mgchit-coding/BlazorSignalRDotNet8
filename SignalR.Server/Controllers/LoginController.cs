using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SignalR.Server.Hubs;
using SignalR.Server.Models;
using SignalR.Server.Services;
using System.Runtime.CompilerServices;

namespace SignalR.Server.Controllers
{
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
            var item = await _context.User
                .FirstOrDefaultAsync(x => x.UserName == requestModel.UserName &&
                                          x.Password == requestModel.Password);
            var loginCount = await _context.Login.CountAsync();

            LoginDataModel model = new LoginDataModel();
            if (item is not null)
            {
                model = new LoginDataModel
                {
                    UserId = item.GeneratedUserId,
                    SessionId = Guid.NewGuid().ToString()
                };
                await _context.Login.AddAsync(model);
                await _context.SaveChangesAsync();
                if (loginCount > 0)
                {
                    await _chatHub.PushNotification(model);
                }
            }

            var responseModel = new ResponseModel()
            {
                ResponseData = JsonConvert.SerializeObject(model),
                ResponseMessage = item is not null ? "Success" : "Fail"
            };
            return Ok(responseModel);
        }
    }

    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}