using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Models
{
  public class UserRole : IdentityModel
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public int Level { get; set; }
    public string Role { get; set; }
  }
}
