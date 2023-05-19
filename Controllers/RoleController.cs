using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/roles")]
  [Authorize(Roles = "Admin")]

  public class RoleController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<RoleController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public RoleController(TeczkaCoreContext teczkacoreContext, ILogger<RoleController> logger, IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Role>> Get()
    {
      try
      {
        var roles = _teczkacoreContext.Roles.ToArray();
        if (roles == null)
        {
          return BadRequest("Fail access to 'Role' table");
        }
        return Ok(roles);
      }
      catch (Exception ex)
      {
          return BadRequest(ex.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Role> Get(int id)
    {
        var role = _teczkacoreContext.Roles
            .FirstOrDefault(m => m.Id == id);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role);
    }

    [HttpPost]
    public ActionResult Post([FromBody] Role model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _teczkacoreContext.Roles.Add(model);
        _teczkacoreContext.SaveChanges();

        int id = model.Id;
        return Created("api/role/" + id, id);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Role model)
    {
        var role = _teczkacoreContext.Roles
            .FirstOrDefault(m => m.Id == id);

        if (role == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        role.Name = model.Name;
        role.Level = model.Level;

        _teczkacoreContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var role = _teczkacoreContext.Roles
            .FirstOrDefault(m => m.Id == id);

        if (role == null)
        {
            return NotFound();
        }

        _teczkacoreContext.Remove(role);
        _teczkacoreContext.SaveChanges();

        return NoContent();
    }

  }
}
