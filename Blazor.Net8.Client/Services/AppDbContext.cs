using Microsoft.EntityFrameworkCore;
using SignalR.Server.Models;
using NotificationDataModel = Blazor.Net8.Client.Models.NotificationDataModel;


namespace Blazor.Net8.Client.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<LoginDataModel> Login {  get; set; }
    public DbSet<UserDataModel> User {  get; set; }
    public DbSet<NotificationDataModel> Notification {  get; set; }
}
