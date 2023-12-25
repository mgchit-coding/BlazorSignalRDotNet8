using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorDotNet8.Models;

[Table("Tbl_User")]
public class UserDataModel
{
    [Key]
    public int UserId { get; set; }
    public string GeneratedUserId { get; set; } 
    public string UserName { get; set; }
    public string Password { get; set; }
    public string UserType { get; set; }
}