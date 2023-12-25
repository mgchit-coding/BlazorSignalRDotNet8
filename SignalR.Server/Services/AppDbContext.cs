using Microsoft.EntityFrameworkCore;
using BlazorDotNet8.Models;

namespace SignalR.Server.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<LoginDataModel> Login {  get; set; }
    public DbSet<UserDataModel> User {  get; set; }
    public DbSet<NotificationDataModel> Notification {  get; set; }
}
