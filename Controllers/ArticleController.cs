using TeczkaCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Controllers
{
  [ApiController]
  [Route("api/articles")]
  [Authorize(Roles = "Admin")]
  public class ArticleController : ControllerBase
  {
    private readonly TeczkaCoreContext _teczkacoreContext;
    private readonly ILogger<ArticleController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public ArticleController(TeczkaCoreContext teczkacoreContext, ILogger<ArticleController> logger, IAuthorizationService authorizationService)
    {
      _teczkacoreContext = teczkacoreContext;
      _logger = logger;
      _authorizationService = authorizationService;
    }

    // GET: api/<ArticleController>
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<List<Models.Article>> Get()
    {
      try
      {
        var articles = _teczkacoreContext.Articles;
        if (articles == null)
        {
          return BadRequest("Fail access to 'Article' table");
        }
        return Ok(articles.ToArray());
      } 
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    // GET api/<ArticleController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<ArticleController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<ArticleController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ArticleController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
