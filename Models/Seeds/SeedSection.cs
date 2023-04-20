using System.Collections;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Text;
using TeczkaCore.Controllers;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Data;

namespace TeczkaCore.Models.Seeds
{
  public class SeedSection
  {
    private TeczkaCoreContext context;
    private ILogger<RoleController>? _logger;

    public SeedSection(TeczkaCoreContext dbContext, ILogger<RoleController> logger)
    {
      context = dbContext;
      _logger = logger;
    }

    public int SeedTable(WebApplication app)
    {
      try
      {
        context.Database.EnsureCreated();
        if (!context.Sections.Any())
        {
          _logger?.LogInformation("Przygotowanie pierwszej informacji do umieszczenia w tablicy 'Section'");
          Article[] articles = context.Articles.ToArray();
          String[] files = Directory.GetFiles("E:\\ZZS\\VS\\Teczka\\teczka\\src\\assets\\json", "*.json");
          foreach (string file in files)
          {
            Section section = new Section();
            section.Name = Path.GetFileNameWithoutExtension(file);
            section.Name = section.Name.Replace('.', '/');
            foreach (Article article in articles)
            {
              if (section.Name.StartsWith(article.Name))
              {
                section.ArticleId = article.Id;
              }
            }
            string jsonData = File.ReadAllText(file);
            using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonData)))
            {
              string name = "";
              while (reader != null && reader.Read())
              {
                if (reader.Value != null)
                {
                  if (reader.TokenType == JsonToken.PropertyName)
                  {
                    name = reader.Value.ToString();
                  }
                  if (reader.TokenType == JsonToken.String)
                  {
                    string value = reader.Value.ToString();
                    value = value ?? "";
                    switch (name)
                    {
                      case "thumbs":
                        section.Thumbs = value;
                        break;
                      case "pages":
                        section.Pages = value;
                        break;
                      case "pdf":
                        section.Pdf = value;
                        break;
                      case "header":
                        section.Header = value;
                        break;
                      case "description":
                        section.Description = value;
                        break;
                      case "offset":
                        section.Offset = int.Parse(value);
                        break;
                      case "min":
                        section.Min = int.Parse(value);
                        break;
                      case "max":
                        section.Max = int.Parse(value);
                        break;
                    }
                  }
                }
              }
            }
            context.Sections.Add(section);
            context.SaveChanges();
          }
          return context.Sections.Count();
        }
        else
        {
          _logger?.LogInformation("Tablica 'Section' już była dostatecznie zasilona");
        }
      }
      catch (Exception ex)
      {
          _logger?.LogInformation($"Błąd inicjacji tablicy 'Section' {ex.Message}");
      }
      return 0;
  }

  }
}
