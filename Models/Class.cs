using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Models
{
  public class Class : CreatedModel
  {
    [MaxLength(2)]
    [Required]
    public string Name { get; set; }
    [Required]
    public List<Person> Persons { get; set; } = new List<Person>();
    public Class()
    {
      this.Name = "";
    }
    public Class(string _name)
    {
      this.Name = _name;
    }
  }
}
