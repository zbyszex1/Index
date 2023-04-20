using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Models.Interfaces
{
    public class CreatedModel : ICreatedModel, IIdentityModel
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
  }
}
