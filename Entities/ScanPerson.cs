using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeczkaCore.Models;

namespace TeczkaCore.Entities
{
  public class ScanPerson
  {
    public int? Id { get; set; }
    public int? UserId { get; set; }
    public int? SectionId { get; set; }
    public int? Page { get; set; }
    public bool? Done { get; set; }
    public List<int> Persons { get; set; } = new List<int>();
  }
}
