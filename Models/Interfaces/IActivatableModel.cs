using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TeczkaCore.Models.Interfaces
{
  public interface IActivatableModel
  {
    [Required]
    public bool IsActive { get; set; }
    [AllowNull]
    public int UserId { get; set; }
  }
}
