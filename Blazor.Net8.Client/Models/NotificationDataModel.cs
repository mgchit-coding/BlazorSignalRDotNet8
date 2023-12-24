using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Net8.Client.Models
{
    [Table("Tbl_Notification")]
    public class NotificationDataModel
    {
        [Key]
        public int Id { get; set; }
        public int LoginId { get; set; }
        public string ConnectionId { get; set; }
    }
}
