using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TeczkaCore.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/persons")]
  public class PersonController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<PersonController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public PersonController(TeczkaCoreContext teczkacoreContext, ILogger<PersonController> logger, IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }


    [HttpGet]
    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    public ActionResult<List<Models.Person>> Get()
    {
      try
      {
        var persons = _teczkacoreContext.Persons;
        if (persons == null)
        {
          return BadRequest("Fail access to 'Person' table");
        }
        return Ok(persons.ToArray());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Person> Get(int id)
    {
      var person = _teczkacoreContext.Persons
          .FirstOrDefault(m => m.Id == id);

      if (person == null)
      {
        return NotFound();
      }

      return Ok(person);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Reader,Writer,Superviser")]
    public ActionResult Post([FromBody] Person person)
    {
      person.Last = person.Last.Trim();
      person.First = person.First.Trim();
      if (!ModelState.IsValid || person.Last.Length<3 || person.First.Length<3)
      {
        return BadRequest(ModelState);
      }

      person.Last = person.Last.Replace('-', ' ');
      person.First = person.First.Replace(' ', '_');
      string userStr = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
      int userId = int.Parse(userStr);
      person.UserId = userId;
      try
      {
        _teczkacoreContext.Persons.Add(person);
        _teczkacoreContext.SaveChanges();
      }
      catch(Exception ex)
      {
        return BadRequest(ex.Message + "; sprawdź czy nie dublujesz osoby");
      }
      return Ok(person);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Person _person)
    {
      var person = _teczkacoreContext.Persons
          .FirstOrDefault(p => p.Id == id);

      if (person == null)
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      person.Last = _person.Last;
      person.First = _person.First;

      _teczkacoreContext.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
      var person = _teczkacoreContext.Persons
          .FirstOrDefault(m => m.Id == id);

      if (person == null)
      {
        return NotFound();
      }

      _teczkacoreContext.Remove(person);
      _teczkacoreContext.SaveChanges();

      return NoContent();
    }

  }
}
