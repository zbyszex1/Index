using TeczkaCore.Models;

namespace TeczkaCore.Entities
{
  public class JsonPage
  {
    public string Header { get; set; }
    public string Pages { get; set; }
    public int personId { get; set; }
    public int Count { get; set; }
    public string Class { get; set; }
    public Unit[] Units { get; set; }
  }

}
