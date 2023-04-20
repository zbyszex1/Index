using System.ComponentModel.DataAnnotations;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Models
{
  public class Indeks : CreatedModel
  {
    public int PersonId { get; set; }
    public int ScanId { get; set; }
    public int UserId { get; set; }
    public Person Person { get; set; }
    public Scan Scan { get; set; }
    public User User { get; set; }

    public Indeks()
    {
      this.PersonId = 1;
      this.ScanId = 1;
      this.UserId = 1;
      this.Person = null;
      this.Scan = null;
      this.User = null;
    }
  }
}
