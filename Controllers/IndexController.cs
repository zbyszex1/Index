using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/indexes")]
  [Authorize(Roles = "Admin")]
  public class IndexController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<IndexController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public IndexController(TeczkaCoreContext teczkacoreContext, ILogger<IndexController> logger, IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Models.Indeks>> Get()
    {
      try
      {
        var indexes = _teczkacoreContext.Indexes;
        if (indexes == null)
        {
          return BadRequest("Fail access to 'Index' table");
        }
        return Ok(indexes.ToArray());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Models.Indeks> Get(int id)
    {
      var index = _teczkacoreContext.Indexes
          .FirstOrDefault(m => m.Id == id);

      if (index == null)
      {
        return NotFound();
      }

      return Ok(index);
    }

    [HttpPost]
    public ActionResult Post([FromBody] Models.Indeks model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _teczkacoreContext.Indexes.Add(model);
      _teczkacoreContext.SaveChanges();

      int id = model.Id;
      return Created("api/index/" + id, id);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Models.Indeks model)
    {
      var index = _teczkacoreContext.Indexes
          .FirstOrDefault(m => m.Id == id);

      if (index == null)
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      index.PersonId = model.PersonId;
      index.ScanId = model.ScanId;

      _teczkacoreContext.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
      var index = _teczkacoreContext.Indexes
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
}
