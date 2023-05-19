using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeczkaCore.Models;
using System.IO.Pipes;

namespace TeczkaCore.Entities
{
  public class Pagination
  {
    public string? Filter { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
  }

}
