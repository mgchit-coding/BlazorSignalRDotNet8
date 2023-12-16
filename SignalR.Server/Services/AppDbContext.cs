using Microsoft.EntityFrameworkCore;
using SignalR.Server.Models;

namespace SignalR.Server.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<LoginModel> login {  get; set; }
}
