using TeczkaCore.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeczkaCore.Entities
{
  public class ServerData
  {
    public string Name { get; set; }
    public string Value { get; set; }

    public ServerData(string _name, string _value)
    {
      this.Name = _name;
      this.Value = _value;
    }
  }
}
