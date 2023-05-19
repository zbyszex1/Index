using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/classes")]
  [Authorize(Roles = "Admin")]

  public class ClassController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<ClassController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public ClassController(TeczkaCoreContext teczkacoreContext, ILogger<ClassController> logger, IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Class>> Get()
    {
      try
      {
        var classes = _teczkacoreContext.Classes;
        if (classes == null)
        {
          return BadRequest("Fail access to 'Class' table");
        }
        return Ok(classes.ToList());
      }
      catch (Exception ex)
      {
          return BadRequest(ex.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Class> Get(int id)
    {
        var Class = _teczkacoreContext.Classes
            .FirstOrDefault(m => m.Id == id);

        if (Class == null)
        {
            return NotFound();
        }

        return Ok(Class);
    }

    [HttpPost]
    public ActionResult Post([FromBody] Class model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _teczkacoreContext.Classes.Add(model);
        _teczkacoreContext.SaveChanges();

        int id = model.Id;
        return Created("api/Class/" + id, id);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Class model)
    {
        var Class = _teczkacoreContext.Classes
            .FirstOrDefault(m => m.Id == id);

        if (Class == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Class.Name = model.Name;

        _teczkacoreContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var Class = _teczkacoreContext.Classes
            .FirstOrDefault(m => m.Id == id);

        if (Class == null)
        {
            return NotFound();
        }

        _teczkacoreContext.Remove(Class);
        _teczkacoreContext.SaveChanges();

        return NoContent();
    }

  }
}
