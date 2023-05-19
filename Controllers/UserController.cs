using Microsoft.AspNetCore.Mvc;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TeczkaCore.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly TeczkaCoreContext _teczkacoreContext;
        private readonly ILogger<UserController> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthorizationService _authorizationService;

        public UserController(TeczkaCoreContext teczkacoreContext, ILogger<UserController> logger, IPasswordHasher<User> passwordHasher, IAuthorizationService authorizationService)
        {
            _teczkacoreContext = teczkacoreContext;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            try
            {
              var users = _teczkacoreContext.Roles.Join(
                inner: _teczkacoreContext.Users,
                outerKeySelector: Role => Role.Id,
                innerKeySelector: User => User.RoleId,
                resultSelector: (r, u) =>
                //new NewRecord(u.Id, u.Name, u.Email, u.Phone, RoleName = r.Name)
                new { u.Id, u.Name, u.Email, u.Phone, u.TempPassword, u.PasswordHash, u.Created, u.Updated, RoleName = r.Name }
              ).AsEnumerable().OrderBy(r => r.RoleName).ThenBy(u => u.Name);
              return Ok(users.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _teczkacoreContext.Users
                .FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult Post([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _teczkacoreContext.Users.Add(model);
            _teczkacoreContext.SaveChanges();

            int id = model.Id;
            return Created("api/user/" + id, id);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User model)
        {
            var user = _teczkacoreContext.Users
                .FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.Phone = model.Phone;

            _teczkacoreContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = _teczkacoreContext.Users
                .FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _teczkacoreContext.Remove(user);
            _teczkacoreContext.SaveChanges();

            return NoContent();
        }

    }

  internal record NewRecord(int Id, string Name, string Email, string? Phone, string Item);
}
