using System.Collections;
using TeczkaCore.Controllers;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TeczkaCore.Models.Seeds
{
  public class SeedRole
  {
    private TeczkaCoreContext context;
    private ILogger<RoleController>? _logger;

    public SeedRole(TeczkaCoreContext dbContext, ILogger<RoleController> logger)
    {
      context = dbContext;
      _logger = logger;
    }

    public int SeedTable(WebApplication app)
    {
      try
      {
        context.Database.EnsureCreated();
        if (!context.Roles.Any())
        {
          _logger?.LogInformation("Przygotowanie pierwszej informacji do umieszczenia w tablicy 'Role'");
          Role[] roles =
          {
            new Role(1, "Reader"),
            new Role(2, "Writer"),
            new Role(3, "Superviser"),
            new Role(4, "Admin"),
          };
          context.Roles.AddRange(roles);
          context.SaveChanges();
          return context.Roles.Count();
        }
        else
        {
          _logger?.LogInformation("Tablica 'Role' już była dostatecznie zasilona");
        }
      }
      catch (Exception ex)
      {
        _logger?.LogInformation($"Błąd inicjacji tablicy 'Role' {ex.Message}");
      }
      return 0;
    }
  }
}
