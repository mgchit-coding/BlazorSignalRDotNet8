namespace BlazorDotNet8.Models;

public class LoginViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? ConnectionId { get; set; }
    public bool Status { get; set; }
}