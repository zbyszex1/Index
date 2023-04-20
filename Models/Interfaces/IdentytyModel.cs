using System.ComponentModel.DataAnnotations;

namespace TeczkaCore.Models.Interfaces
{
  public class IdentityModel : IIdentityModel
  {
    [Required]
    public int Id { get; set; }
  }
}
