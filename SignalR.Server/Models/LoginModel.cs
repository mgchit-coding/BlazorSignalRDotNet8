using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR.Server.Models
{
    [Table("Tbl_TestLogin")]
    public class LoginModel
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; }
        public string Status {  get; set; }
    }
}
