using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
            var isActive = await _context.login
                .FirstOrDefaultAsync(x => x.UserName == requestModel.UserName &&
                x.Password == requestModel.Password);
            if (!isActive.Status)
            {
                isActive.Status = true;
                _context.Update(isActive);
                await _context.SaveChangesAsync();
                await _chatHub.CreateUserConnection(isActive);
            }
            else
            {
                await _chatHub.SendNotification(isActive);
            }
            return Ok(isActive);
        }
        
        [HttpGet]
        public async Task<IActionResult> UpdateUserStatus()
        {
            var isActive = await _context.login
                .FirstOrDefaultAsync(x => x.UserName == "mgchit" &&
                x.Password == "123");
            isActive.Status = false;
            _context.Update(isActive);
            await _context.SaveChangesAsync();
            return Ok(isActive);
        }
    }

    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
