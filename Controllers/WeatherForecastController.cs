using TeczkaCore.Models;
using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TeczkaCore.Entities;
using System.Linq;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<AvailableController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public WeatherForecastController(TeczkaCoreContext teczkacoreContext, ILogger<AvailableController> logger,
                                     IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }

    [HttpGet("weatherforecast")]
    public IEnumerable<WeatherForecast> Get()
    {
      return Enumerable.Range(1, 10).Select(index => new WeatherForecast
      {
        Date = DateTime.UtcNow.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }

    [HttpPost("generate")]
    public async Task<ActionResult> Generate()
    {
      List<JsonIndeks> jsonIndekses = new List<JsonIndeks>();
      Indeks[] indekses = _teczkacoreContext.Indexes.OrderBy(i => i.PersonId).ThenBy(i => i.ScanId).ToArray<Indeks>();
      Scan[] scans = _teczkacoreContext.Scans.OrderBy(s => s.SectionId).ThenBy(s => s.Page).ToArray<Scan>();
      Section[] sections = _teczkacoreContext.Sections.OrderBy(s => s.ArticleId).ThenBy(s => s.Name).ToArray<Section>();
      Person[] persons = _teczkacoreContext.Persons.OrderBy(p => p.Last).ThenBy(p => p.First).ToArray<Person>();
      foreach (Person person in persons)
      {
        JsonIndeks jsonIndeks = new JsonIndeks();
        jsonIndeks.Name = person.Last.Replace('-', ' ') + " " + person.First.Replace(' ', '_');
        int personId = person.Id;
        var selIndekses = indekses.Where(i => i.PersonId == personId);
        string display = "";
        List<int> pages = new List<int>();
        List<Unit> Units = new List<Unit>();
        Unit Unit = null;
        foreach (Indeks indeks in selIndekses)
        {
          int scanId = indeks.ScanId;
          Scan scan = _teczkacoreContext.Scans.Find(scanId);
          if (scan == null)
            continue;
          Section section = _teczkacoreContext.Sections.Find(scan.SectionId);
          if (section == null)
            continue;
          string path = section.Name.ToLower();
          string display0 = path;
          int sp = display0.LastIndexOf("/");
          if (sp >= 0)
            display0 = display0.Substring(++sp);
          if (display0 != display)
          {
            if (Unit != null)
            {
              Unit.Pages = pages.ToArray();
              Units.Add(Unit);
            }
            Unit = new Unit();
            pages = new List<int>();

            display = display0;
            Unit.Display = display;
            Unit.Path = path;
          }
          pages.Add(scan.Page);
        }
        if (pages.Count == 0)
          continue;
        Unit.Pages = pages.ToArray();
        Units.Add(Unit);
        jsonIndeks.Units = Units.ToArray();
        jsonIndekses.Add(jsonIndeks);
      }
      return Ok(jsonIndekses.ToArray());
    }

  }
}