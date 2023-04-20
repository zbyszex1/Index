using System.ComponentModel.DataAnnotations;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Models
{
  public class Section : CreatedModel
  {
    public int ArticleId { get; set; }
    public string Name { get; set; }
    public string Thumbs { get; set; }
    public string Pages { get; set; }
    public string Pdf { get; set; }
    public string Header { get; set; }
    public string Description { get; set; }
    public int Offset { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public Article Article { get; set; }
    public List<Scan> Scans { get; set; } = new List<Scan>();

    public Section()
    {
    }
    public Section(int _articleId, string _name)
    {
      this.ArticleId = _articleId;
      this.Name = _name;
    }
  }
}
