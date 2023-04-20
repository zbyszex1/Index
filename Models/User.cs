using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Models
{
  public class User : CreatedModel
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? TempPassword { get; set; }
    public string? PasswordHash { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public List<Scan> Scans { get; set; } = new List<Scan>();
    public List<Person> Persons { get; set; } = new List<Person>();
    public List<Indeks> Indexes { get; set; } = new List<Indeks>();

  }
}
