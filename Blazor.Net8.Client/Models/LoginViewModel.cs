namespace Blazor.Net8.Client
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? ConnectionId { get; set; }
        public bool Status { get; set; }
    }
}
