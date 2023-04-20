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
  //[Authorize(Roles = "Admin")]
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
        var scans = _teczkacoreContext.Scans;
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
