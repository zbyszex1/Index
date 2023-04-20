﻿using TeczkaCore.Entities;
using TeczkaCore.Identity;
using TeczkaCore.Models;
using TeczkaCore.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using System.Data;

namespace TeczkaCore.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/account")]
  [Authorize(Roles = "Admin,Reader,Writer,Superviser")]

  public class AccountController : ControllerBase
  {
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly TeczkaCoreContext context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public AccountController(TeczkaCoreContext TeczkaCoreContext, IPasswordHasher<User> passwordHasher, IJwtProvider jwtProvider)
    {
      context = TeczkaCoreContext;
      _passwordHasher = passwordHasher;
      _jwtProvider = jwtProvider;
    }
    internal class Token
    {
      public string serverToken { get; set; }
    }

    // ================================================================
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
    {
      if (!ModelState.IsValid || context == null || _passwordHasher == null)
      {
        return BadRequest(ModelState);
      }

      var user = await context.Users
        .Include(user => user.Role)
        .FirstOrDefaultAsync(user => user.Email == userLogin.Email);

      if (user == null)
      {
        return BadRequest("Invalid username or password");
      }

      var passwordVeificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLogin.Password);
      if (passwordVeificationResult == PasswordVerificationResult.Failed)
      {
        if (user.TempPassword != userLogin.Password)
        {
          return BadRequest("Invalid username or password");
        }
      }

      Token token = new Token();
      token.serverToken = _jwtProvider.GenerateJwtToken(user);
      return Ok(token);
    }

    // ================================================================
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] UserRegister registerUser)
    {
      if (!ModelState.IsValid || context == null || _passwordHasher == null)
      {
        return BadRequest(ModelState);
      }

      var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Writer");

      int roleId = role != null ? role.Id : 1;
      var newUser = new User()
      {
        Name = registerUser.Name,
        Email = registerUser.Email,
        Phone = registerUser.Phone,
        TempPassword = null,
        RoleId = roleId
      };
      newUser.PasswordHash = _passwordHasher.HashPassword(newUser, registerUser.Password);
      newUser.RoleId = role.Id;
      context.Users.Add(newUser);
      await context.SaveChangesAsync();

      Token token = new Token();
      token.serverToken = _jwtProvider.GenerateJwtToken(newUser);
      return Ok(token);
    }

    // ================================================================
    [HttpPost("{id}")]
    public async Task<ActionResult> Password([FromBody] UserPassword passwordUser)
    {
      if (!ModelState.IsValid || context == null || _passwordHasher == null)
      {
        return BadRequest(ModelState);
      }

      if (passwordUser.Password != null && passwordUser.Confirm != null && passwordUser.Old != null &&
        passwordUser.Password == passwordUser.Confirm &&
        passwordUser.Password.Length > 0 && passwordUser.Old.Length > 0)
      {
        var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = context.Users
          .Include(user => user.Role)
          .FirstOrDefault(user => user.Id.ToString() == userId);
        if (user == null)
        {
          return BadRequest(ModelState);
        }
        var passwordVeificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordUser.Old);
        if (passwordVeificationResult == PasswordVerificationResult.Failed)
        {
          if (user.TempPassword != passwordUser.Old)
          {
            return BadRequest("Invalid password");
          }
        }
        user.TempPassword = "";
        var passwordHash = _passwordHasher.HashPassword(user, passwordUser.Password);
        user.PasswordHash = passwordHash;
        await context.SaveChangesAsync();

        Token token = new Token();
        token.serverToken = _jwtProvider.GenerateJwtToken(user);
        return Ok(token);
      }
      return BadRequest(ModelState);
    }

    // ================================================================
    [HttpPut("{id}")]
    public async Task<ActionResult> Personal([FromBody] UserPersonal personalUser)
    {
      if (!ModelState.IsValid || context == null || _passwordHasher == null)
      {
        return BadRequest(ModelState);
      }

      if ((personalUser.Name == null && personalUser.Email == null && personalUser.Phone == null) ||
        (personalUser.Name.Length == 0 && personalUser.Email?.Length == 0 && personalUser.Phone?.Length == 0))
      {
        return BadRequest(ModelState);
      }
      var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
      var user = context.Users
        .Include(user => user.Role)
        .FirstOrDefault(user => user.Id.ToString() == userId);
      if (user == null)
      {
        return BadRequest(ModelState);
      }
      if (personalUser.Name.Length > 0)
        user.Name = personalUser.Name;
      if (personalUser.Email.Length > 0)
        user.Email = personalUser.Email;
      if (personalUser.Phone.Length > 0)
        user.Phone = personalUser.Phone;
      await context.SaveChangesAsync();

      Token token = new Token();
      token.serverToken = _jwtProvider.GenerateJwtToken(user);
      return Ok(token);
    }
  }
  // ================================================================

}
