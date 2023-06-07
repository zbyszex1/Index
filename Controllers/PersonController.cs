using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TeczkaCore.Entities;
using System.Linq;

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
        var persons = _teczkacoreContext.PersonsClasses.ToList();
        //var persons = _teczkacoreContext.Classes.Join(
        //  inner: _teczkacoreContext.Persons,
        //  outerKeySelector: Class => Class.Id,
        //  innerKeySelector: Person => Person.ClassId,
        //  resultSelector: (c, p) =>
        //  new { p.Id, p.Last, p.First, p.UserId, ClassName = c.Name, p.Created, p.Updated }
        //).AsEnumerable().OrderBy(c => c.ClassName).ThenBy(p => p.Last).ThenBy(p => p.First);
        return Ok(persons.ToList());
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
        int count = _teczkacoreContext.Persons.Count();

        return Ok(count);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("grp")]
    [AllowAnonymous]
    public ActionResult Get(string any)
    {
      try
      {
        var personsGroups = _teczkacoreContext.PersonsGroups;

        return Ok(personsGroups.ToList());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("pg")]
    [AllowAnonymous]
    public ActionResult<List<Person>> Get(string? filter, int Page, int PageSize)
    {
      try
      {
        var persons = _teczkacoreContext.PersonsClasses
        .Where(p => filter != null ? (p.Last.Contains(filter) || p.First.Contains(filter)) : true)
        .Skip(Page * PageSize)
        .Take(PageSize);
        //var persons = _teczkacoreContext.Classes.Join(
        //  inner: _teczkacoreContext.Persons,
        //  outerKeySelector: Class => Class.Id,
        //  innerKeySelector: Person => Person.ClassId,
        //  resultSelector: (c, p) =>
        //  new { p.Id, p.Last, p.First, p.UserId, Class = c.Name, p.Created, p.Updated }
        //)
        //.Where(p=> filter!=null ? (p.Last.Contains(filter) || p.First.Contains(filter)) :true)
        //.AsEnumerable()
        //.OrderBy(p => p.Last)
        //.ThenBy(p => p.First)
        //.Skip(Page * PageSize)
        //.Take(PageSize);

        return Ok(persons.ToList());
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
    public ActionResult Post([FromBody] PersonCreate personCreate)
    {
      Person person = new Person();
      personCreate.Last = personCreate.Last.Replace('-', ' ');
      personCreate.First = personCreate.First.Replace(' ', '_');
      person.Last = personCreate.Last.Trim();
      person.First = personCreate.First.Trim();
      if (!ModelState.IsValid || person.Last.Length<3 || person.First.Length<3)
      {
        return BadRequest(ModelState);
      }

      string userStr = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
      int userId = int.Parse(userStr);
      person.UserId = userId;
      person.ClassId = 1;
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
    [Authorize(Roles = "Admin,Superviser")]
    public ActionResult Put(int id, [FromBody] PersonUpdate _person)
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

      if (_person.Last != null)
      {
        person.Last = _person.Last;
      }
      if (_person.First != null)
      {
        person.First = _person.First;
      }
      if (_person.ClassId != 0)
      {
        person.ClassId = _person.ClassId;
      }
      string userStr = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
      int userId = int.Parse(userStr);
      person.UserId = userId;
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
