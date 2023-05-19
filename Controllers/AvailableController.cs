using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TeczkaCore.Entities;
using TeczkaCore.Services;
using static TeczkaCore.Controllers.AccountController;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/available")]
  [Authorize(Roles = "Admin,Reader,Writer,Superviser")]
  public class AvailableController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<AvailableController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMailService _mailService;

    public AvailableController(TeczkaCoreContext teczkacoreContext, ILogger<AvailableController> logger, 
                               IAuthorizationService authorizationService, IMailService mailService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
      _mailService = mailService;
    }

    [HttpGet]
    public ActionResult<List<Models.Scan>> Get()
    {
      try
      {
        var scans = _teczkacoreContext.Scans.Where(s => s.Done == false);
        var sections = _teczkacoreContext.Sections;
        if (scans == null)
        {
          return BadRequest("Fail access to 'Scan' table");
        }
        if (sections == null)
        {
          return BadRequest("Fail access to 'Section' table");
        }
        Object[] result = { sections.ToArray(), scans.ToArray() };
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    //[AllowAnonymous]
    [HttpGet("{table}")]
    public ActionResult<Models.Scan> Get(string table)
    {
      switch (table)
      {
        case "classes":
          var classes = _teczkacoreContext.Classes;
          if (classes == null)
          {
            return BadRequest("Fail access to 'Classes' table");
          }
          return Ok(classes.ToArray());
          //break;
        case "scans":
          var scans = _teczkacoreContext.Scans.Where(s => s.Done == false);
          if (scans == null)
          {
            return BadRequest("Fail access to 'Scan' table");
          }
          return Ok(scans.ToArray());
          //break;
        case "sections":
          var sections = _teczkacoreContext.Sections.OrderBy(s =>s.Name);
          if (sections == null)
          {
            return BadRequest("Fail access to 'Section' table");
          }
          return Ok(sections.ToArray());
          //break;
        case "persons":
          var persons = _teczkacoreContext.Persons;
          if (persons == null)
          {
            return BadRequest("Fail access to 'Person' table");
          }
          return Ok(persons.OrderBy(p => p.Last).ThenBy(p => p.First).ToArray());
          //break;
        case "test":
          object[] result = { };
          return Ok(result);
          //break;
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

    [AllowAnonymous]
    [HttpPost("email")]
    public async Task<ActionResult> SendEmail(UserEmail model)
    {
      try
      {
        MailRequest request = new MailRequest();
        request.ToEmail = "zbigniew@sarata.pl";
        request.FromEmail = model.Email;
        request.Sender = model.Sender;
        request.Subject = model.Subject.Trim();
        request.Body = model.Message;
        request.Attachments = null;

        await _mailService.SendEmailAsync(request);
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest("Something went wrong: " + ex.Message);
      }
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] ScanPerson model)
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
      if (model.SectionId != null)
        scan.SectionId = (int)model.SectionId;
      if (model.UserId != null)
        scan.UserId = (int)model.UserId;
      if (model.Page != null)
        scan.Page = (int)model.Page;
      if (model.Done != null)
        scan.Done = (bool)model.Done;
      _teczkacoreContext.SaveChanges();

      if (model.Persons != null && model.UserId != null)
      {
        List<Models.Indeks> indexes = new List<Models.Indeks>();
        foreach (int personId in model.Persons)
        {
          Models.Indeks indeks = new Models.Indeks();
          indeks.PersonId = personId;
          indeks.UserId = (int)model.UserId;
          indeks.ScanId = id;
          indexes.Add(indeks);
        }
        _teczkacoreContext.Indexes.AddRange(indexes);
        _teczkacoreContext.SaveChanges();
      }
      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
      var indeks = _teczkacoreContext.Scans
          .FirstOrDefault(m => m.Id == id);

      if (indeks == null)
      {
        return NotFound();
      }

      _teczkacoreContext.Remove(indeks);
      _teczkacoreContext.SaveChanges();

      return NoContent();
    }

  }
}
