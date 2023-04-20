using TeczkaCore.Models;

namespace TeczkaCore.Entities
{
  public class JsonIndeks
  {
    public string Name { get; set; } 
    public Unit[] Units { get; set; }
  }

  public class Unit
  {
    public string Display { get; set;}
    public string Path { get; set; }
    public int[] Pages { get; set; }
  }

}
