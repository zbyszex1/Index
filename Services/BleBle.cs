using TeczkaCore.Services;
namespace TeczkaCore.Services
{
  public class BleBle : IBleBle
  {
    public string Repeat(string plotka)
    {
      return plotka.Trim();
    }

    public string Tomato(string plotka)
    {
      return "Pomidor";
    }
    public string Add(string plotka)
    {
      return plotka + " - takie coś usłyszałem";
    }
  }
}
