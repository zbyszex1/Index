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
  public class SeedScan
  {
    private TeczkaCoreContext context;
    private ILogger<RoleController>? _logger;

    public SeedScan(TeczkaCoreContext dbContext, ILogger<RoleController> logger)
    {
      context = dbContext;
      _logger = logger;
    }

    public int SeedTable(WebApplication app)
    {
      try
      {
        context.Database.EnsureCreated();
        if (!context.Scans.Any())
        {
          _logger?.LogInformation("Przygotowanie pierwszej informacji do umieszczenia w tablicy 'Scan'");
          Section[] sections = context.Sections.ToArray();
          User[] users = context.Users.ToArray();
          foreach (Section section in sections)
          {
            int min = 1;
            int max = 1;
            int userId = users.Any() ? users[0].Id : 1;
            int sectionId = section.Id;
            int.TryParse(section.Min.ToString(), out min);
            int.TryParse(section.Max.ToString(), out max);
            for (int i = min; i <= max; i++)
            {
              Scan scan = new Scan();
              scan.UserId = userId;
              scan.SectionId = sectionId;
              scan.Page = i;
              scan.Done = false;
              context.Scans.Add(scan);
            }
            context.SaveChanges();
          }
          return context.Scans.Count();
        }
        else
        {
          _logger?.LogInformation("Tablica 'Scan' już była dostatecznie zasilona");
        }
      }
      catch (Exception ex)
      {
          _logger?.LogInformation($"Błąd inicjacji tablicy 'Scan' {ex.Message}");
      }
      return 0;
  }

  }
}
