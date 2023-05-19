using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Models
{
  public class Person : CreatedModel
  {
    public string Last { get; set; }
    public string First { get; set; }
    public int UserId { get; set; }
    public int ClassId { get; set; }
    public Class Class { get; set; }
    public List<Indeks> Indexes { get; set; } = new List<Indeks>();

    public Person()
    {
      this.Last = String.Empty;
      this.First = String.Empty;
      this.UserId = 1;
      this.ClassId = 1;
    }
    public Person(string _last, string _first, int _userId, int classId)
    {
      this.Last = _last;
      this.First = _first;
      this.UserId = _userId;
      ClassId = classId;
    }
  }
}
