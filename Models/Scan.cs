using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Models
{
  public class Scan : CreatedModel
  {
    public int SectionId { get; set; }
    public int UserId { get; set; }
    public int Page { get; set; }
    public bool Done { get; set; }
    public Section Section { get; set; }
    public User User { get; set; }
    public List<Indeks> Indexes { get; set; } = new List<Indeks>();


    //public Indeks()
    //{
    //  this.PersonId = 1;
    //  this.SectionId = 1;
    //  this.userId = 1;
    //}
    //public Indeks(int _personId, int _sectionId, int _userId, int _page)
    //{
    //  this.PersonId = _personId;
    //  this.SectionId = _sectionId;
    //  this.UserId = _userId;
    //  this.Page = _page;
    //}
  }
}
