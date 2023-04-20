using System.Collections;
using TeczkaCore.Controllers;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TeczkaCore.Models.Seeds
{
  public class SeedArticle
  {
  private TeczkaCoreContext context;
    private ILogger<ArticleController>? _logger;

    public SeedArticle(TeczkaCoreContext dbContext, ILogger<ArticleController> logger)
    {
      context = dbContext;
        _logger = logger;
    }

    public int SeedTable(WebApplication app)
    {
      try
      {
        context.Database.EnsureCreated();
        if (!context.Articles.Any())
        {
          _logger?.LogInformation("Przygotowanie pierwszej informacji do umieszczenia w tablicy 'Article'");
          String[] files = Directory.GetFiles("E:\\ZZS\\VS\\Teczka\\teczka\\src\\assets\\json", "*.json");
          List<Article> articles = new List<Article>();
          List<string> fNames = new List<string>();
          foreach (string file in files)
          {
            string fName = Path.GetFileNameWithoutExtension(file);
            if (fName.Contains('.'))
            {
              int l = fName.Length;
              fName = fName.Replace('.', '/').Substring(0,--l);
            }
            if (!fNames.Contains(fName))
            {
              fNames.Add(fName);
              articles.Add(new Article(fName));
            }
          }
          context.Articles.AddRange(articles);
          context.SaveChanges();
          return context.Articles.Count();
        }
        else
        {
          _logger?.LogInformation("Tablica 'Article' już była dostatecznie zasilona");
        }
      }
      catch (Exception ex)
      {
        _logger?.LogInformation($"Błąd inicjacji tablicy 'Article' {ex.Message}");
      }
      return 0;
    }
  }
}
