using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Models
{
  public class Role : CreatedModel
  {
    [MaxLength(64)]
    [Required]
    public string Name { get; set; }
    [Required]
    public int? Level { get; set; }
    public List<User> Users { get; set; } = new List<User>();
    public Role()
    {
      this.Level = 0;
      this.Name = "";
    }
    public Role(int _level, string _name)
    {
      this.Level = _level;
      this.Name = _name;
    }
  }
}
