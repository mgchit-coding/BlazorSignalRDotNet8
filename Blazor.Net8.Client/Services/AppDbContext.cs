using Blazor.Net8.Client.Models;
using Microsoft.EntityFrameworkCore;


namespace Blazor.Net8.Client.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<LoginModel> login {  get; set; }
    public DbSet<NotificationDataModel> notification {  get; set; }
}
