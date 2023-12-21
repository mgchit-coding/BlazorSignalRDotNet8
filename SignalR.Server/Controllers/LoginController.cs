using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR.Server.Models;
using SignalR.Server.Services;

namespace SignalR.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
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
            }
            return Ok(isActive);
        }
    }

    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
