using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/sections")]
  [Authorize(Roles = "Admin")]
  public class SectionController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<SectionController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public SectionController(TeczkaCoreContext teczkacoreContext, ILogger<SectionController> logger, IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }

    // GET: api/<SectionsController>
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Models.Section>> Get()
    {
      try
      {
        var sections = _teczkacoreContext.Sections;
        if (sections == null)
        {
          return BadRequest("Fail access to 'Section' table");
        }
        return Ok(sections.ToArray());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    // GET api/<SectionsController>/5
    [HttpGet("{id}")]
    public ActionResult<Models.Section> Get(int id)
    {
      try
      {
        var section = _teczkacoreContext.Sections.Find(id);
        if (section == null)
        {
          return BadRequest("Fail access to 'Section' table");
        }
        return Ok(section);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    // POST api/<SectionsController>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<SectionsController>/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<SectionsController>/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public void Delete(int id)
    {
    }
  }
}
