using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/test")]

  public class TestController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<TestController> _logger;

    public TestController(TeczkaCoreContext teczkacoreContext, ILogger<TestController> logger)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
    }

    [HttpGet]
    public ActionResult<List<FetchData>> Get()
    {
      try
      {
        FetchData[] fetchData =
        {
          new FetchData("2023-03-11 14:31:59.7388573Z",  22, 71, "Balmy"),
          new FetchData("2023-03-12 14:31:59.7388591Z",  7,  44, "Freezing"),
          new FetchData("2023-03-13 14:31:59.7388593Z", 38, 100, "Warm"),
          new FetchData("2023-03-14 14:31:59.7388594Z", -3,  27 ,"Freezing"),
          new FetchData("2023-03-15 14:31:59.7388595Z", 40, 103, "Sweltering"),
          new FetchData("2023-03-16 14:31:59.7388596Z", 21,  69, "Cool"),
          new FetchData("2023-03-17 14:31:59.7388597Z", 34,  93, "Scorching"),
          new FetchData("2023-03-18 14:31:59.7388598Z", -3,  27, "Hot"),
          new FetchData("2023-03-19 14:31:59.7388599Z",-18,   0, "Mild")
        };

        //]
        //Object[] result = { sections.ToArray(), scans.ToArray() };
        return Ok(fetchData.ToList());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("{table}")]
    public ActionResult<Models.Scan> Get(string table)
    {
      switch (table)
      {
        case "scans":
          var scans = _teczkacoreContext.Scans.Where(s => s.Done == false);
          if (scans == null)
          {
            return BadRequest("Fail access to 'Scan' table");
          }
          return Ok(scans.ToArray());
          break;
        case "sections":
          var sections = _teczkacoreContext.Sections.OrderBy(s =>s.Name);
          if (sections == null)
          {
            return BadRequest("Fail access to 'Section' table");
          }
          return Ok(sections.ToArray());
          break;
        case "persons":
          var persons = _teczkacoreContext.Persons;
          if (persons == null)
          {
            return BadRequest("Fail access to 'Person' table");
          }
          return Ok(persons.OrderBy(p => p.Last).ThenBy(p => p.First).ToArray());
          break;
        case "test":
          string result1 = _teczkacoreContext.Database.GetConnectionString();
          object[] result = { result1 };
          return Ok(result);
          break;
        default:
          break;
      }
      return BadRequest("Unexpected table name");
    }

    [HttpPost]
    public ActionResult Post([FromBody] Models.Scan model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _teczkacoreContext.Scans.Add(model);
      _teczkacoreContext.SaveChanges();

      int id = model.Id;
      return Created("api/index/" + id, id);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Models.Scan model)
    {
      var index = _teczkacoreContext.Scans
          .FirstOrDefault(m => m.Id == id);

      if (index == null)
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      //index.PersonId = model.PersonId;
      //index.ScanId = model.ScanId;

      //_teczkacoreContext.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
      var index = _teczkacoreContext.Scans
          .FirstOrDefault(m => m.Id == id);

      if (index == null)
      {
        return NotFound();
      }

      _teczkacoreContext.Remove(index);
      _teczkacoreContext.SaveChanges();

      return NoContent();
    }

  }
  public class FetchData
  {
    public DateTime date;
    public int temparatureC;
    public int temperatureF;
    public string summary;

    public FetchData(string _date, int _temparatureC, int _temparatureF, string _summary)
    {
      date = DateTime.Parse(_date);
      temparatureC = _temparatureC;
      temperatureF = _temparatureF;
      summary = _summary;
    }
  }
}
