using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Net8.Client
{
    [Table("Tbl_TestLogin")]
    public class LoginModel
    {
        [Key]
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? ConnectionId { get; set; }
        public bool Status {  get; set; }
    }
}
