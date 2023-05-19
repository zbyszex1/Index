using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Models
{
  public class RefreshToken : CreatedModel
  {
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
  }
}