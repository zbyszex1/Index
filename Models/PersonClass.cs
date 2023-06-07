using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Models
{
  public class PersonClass : IdentityModel
  {
    public string Class { get; set; }
    public string Last { get; set; }
    public string First { get; set; }
    public int UserId { get; set; }
  }
}
