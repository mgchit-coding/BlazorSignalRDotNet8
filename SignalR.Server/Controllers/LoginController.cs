using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.Server.Hubs;
using SignalR.Server.Models;
using SignalR.Server.Services;

namespace SignalR.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public LoginController(AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost,Route("Login")]
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
                HttpContext.Response.Cookies.Append("UserName", isActive.UserName);
                HttpContext.Session.SetString("UserName", isActive.UserName);

                var cookie = HttpContext.Request.Cookies["UserName"];
                var session = HttpContext.Session.GetString("UserName");
            }
            else
            {
                await _hubContext.Clients.Clients(isActive.ConnectionId)
                .SendAsync("ReceiveMessage", "Dear User",
                "You login with other device");
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
