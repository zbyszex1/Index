using System.Collections;
using TeczkaCore.Controllers;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TeczkaCore.Models.Seeds
{
  public class SeedClass
  {
    private TeczkaCoreContext context;
    private ILogger<ClassController>? _logger;

    public SeedClass(TeczkaCoreContext dbContext, ILogger<ClassController> logger)
    {
      context = dbContext;
      _logger = logger;
    }

    public int SeedTable(WebApplication app)
    {
      try
      {
        context.Database.EnsureCreated();
        if (!context.Classes.Any())
        {
          _logger?.LogInformation("Przygotowanie pierwszej informacji do umieszczenia w tablicy 'Class'");
          Class[] Classs =
          {
            new Class("PZ"),
            new Class("OP"),
            new Class("SB"),
            new Class("TW"),
            new Class("IT"),
          };
          context.Classes.AddRange(Classs);
          context.SaveChanges();
          return context.Classes.Count();
        }
        else
        {
          _logger?.LogInformation("Tablica 'Class' już była dostatecznie zasilona");
        }
      }
      catch (Exception ex)
      {
        _logger?.LogInformation($"Błąd inicjacji tablicy 'Class' {ex.Message}");
      }
      return 0;
    }
  }
}
