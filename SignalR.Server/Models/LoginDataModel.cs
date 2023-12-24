using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR.Server.Models
{
    [Table("Tbl_Login")]
    public class LoginDataModel
    {
        [Key]
        public int LoginId { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string? ConnectionId { get; set; }
    }
}
