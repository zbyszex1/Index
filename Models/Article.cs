using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace TeczkaCore.Models
{
  public class Article : CreatedModel
  {
    public string Name { get; set; }
    public List<Section> Sections { get; set; } = new List<Section>();

    public Article() 
    { 
    }
    public Article(string _name)
    {
      this.Name = _name;
    }
  }
}
