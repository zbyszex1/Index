using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TeczkaCore.Models.Interfaces
{
  public class ActivModel : IActivatableModel, IIdentityModel
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public bool IsActive { get; set; }
    [AllowNull]
    public int UserId { get; set; }
  }
}
