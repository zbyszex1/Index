using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace TeczkaCore.Models.Interfaces
{
  public interface ICreatedModel
  {
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
  }
}
