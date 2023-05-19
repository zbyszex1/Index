using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using TeczkaCore.Entities;
using TeczkaCore.Models;
using System.Data;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/scans")]
  [Authorize(Roles = "Admin,Reader,Writer,Superviser")]
  public class ScanController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<ScanController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public ScanController(TeczkaCoreContext teczkacoreContext, ILogger<ScanController> logger, IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Models.Scan>> Get()
    {
      try
      {
        //var scans = _teczkacoreContext.Scans;
        var scans = _teczkacoreContext.Sections.Join(
          inner: _teczkacoreContext.Scans,
          outerKeySelector: Section => Section.Id,
          innerKeySelector: Scan => Scan.SectionId,
          resultSelector: (sc, s) =>
          new { s.Id, s.SectionId, Section = sc.Name, s.UserId, s.Page, s.Created, s.Updated }
        )
        .AsEnumerable()
        .OrderBy(s => s.SectionId)
        .ThenBy(s => s.Page);
        if (scans == null)
        {
          return BadRequest("Fail access to 'Scan' table");
        }
        return Ok(scans.ToArray());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("cnt")]
    [AllowAnonymous]
    public ActionResult Get(Boolean any)
    {
      try
      {
        int count = _teczkacoreContext.Scans.Count();

        return Ok(count);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("pg")]
    [AllowAnonymous]
    public ActionResult<List<Scan>> Get(int Page, int PageSize)
    {
      try
      {
        var scans = _teczkacoreContext.Sections.Join(
          inner: _teczkacoreContext.Scans,
          outerKeySelector: Section => Section.Id,
          innerKeySelector: Scan => Scan.SectionId,
          resultSelector: (sc, s) =>
          new { s.Id, s.SectionId, Section = sc.Name, s.UserId, s.Page,  s.Created, s.Updated }
        )
        .AsEnumerable()
        .OrderBy(s => s.SectionId)
        .ThenBy(s => s.Page)
        .Skip(Page * PageSize)
        .Take(PageSize);

        if (scans == null)
        {
          return BadRequest("Fail access to 'Index' table");
        }
        return Ok(scans.ToArray());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Models.Scan> Get(int id)
    {
      var scan = _teczkacoreContext.Scans
          .FirstOrDefault(m => m.Id == id);

      if (scan == null)
      {
        return NotFound();
      }

      return Ok(scan);
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
      return Created("api/scan/" + id, id);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Models.Scan model)
    {
      var scan = _teczkacoreContext.Scans
          .FirstOrDefault(m => m.Id == id);

      if (scan == null || model == null)
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      scan.UserId = model.UserId;
      scan.SectionId = model.SectionId;
      scan.Page = model.Page;
      scan.Done = model.Done;

      _teczkacoreContext.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
      var scan = _teczkacoreContext.Scans
          .FirstOrDefault(m => m.Id == id);

      if (scan == null)
      {
        return NotFound();
      }

      _teczkacoreContext.Remove(scan);
      _teczkacoreContext.SaveChanges();

      return NoContent();
    }

  }
}
