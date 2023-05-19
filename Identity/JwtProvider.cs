using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeczkaCore.Models;
using Microsoft.IdentityModel.Tokens;
using TeczkaCore.Models.Interfaces;

namespace TeczkaCore.Identity
{
  public class JwtProvider : IJwtProvider
  {
    private readonly JwtOptions _jwtOptions;
    private readonly TeczkaCoreContext _context;

    public JwtProvider(JwtOptions jwtOptions, TeczkaCoreContext TeczkaCoreContext)
    {
      _jwtOptions = jwtOptions;
      _context = TeczkaCoreContext;
    }
    // ================================================================
    public string GenerateJwtToken(User user)
    {
      var claims = new List<Claim>()
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.MobilePhone, user.Phone),
        new Claim(ClaimTypes.Role, user.Role.Name),
        //new Claim(ClaimTypes.UserData, GenerateRefreshJwtToken(user.Id)),
        //new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("dd-MM-yyyy")),
      };

      //if (!string.IsNullOrEmpty(user.Nationality))
      //{
      //    claims.Add(
      //        new Claim("Nationality", user.Nationality)
      //    );
      //}

      SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
      SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      DateTime expiresJwt = DateTime.UtcNow.AddMinutes(_jwtOptions.JwtExpireMinutes);
      //DateTime expiresJwt = DateTime.UtcNow;
      ////expires = expires.AddMinutes(2000);

      JwtSecurityToken token = new JwtSecurityToken(
        _jwtOptions.JwtIssuer,
        _jwtOptions.JwtIssuer,
        claims,
        expires: expiresJwt,
        signingCredentials: creds
      );

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      return tokenHandler.WriteToken(token);
    }

    // ================================================================
    public string GenerateRefreshJwtToken(int userId)
    {
      var claims = new List<Claim>()
      {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
      };
      SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtRefreshKey));
      SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      DateTime expiresRefreshJwt = DateTime.UtcNow.AddMinutes(_jwtOptions.JwtRefreshExpireMinutes);
      JwtSecurityToken token = new JwtSecurityToken(
        _jwtOptions.JwtIssuer,
        _jwtOptions.JwtIssuer,
        claims,
        expires: expiresRefreshJwt,
        signingCredentials: creds
      );

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      string refreshToken = tokenHandler.WriteToken(token).ToString();
      Boolean bNew = false;
      var rt = _context.RefreshTokens.SingleOrDefault(t => t.UserId == userId);
      if (rt == null)
      {
        rt = new RefreshToken();
        bNew = true;
      }
      rt.Token = refreshToken;
      rt.UserId = userId;
      rt.Expires = expiresRefreshJwt;
      if (bNew)
      {
        _context.RefreshTokens.Add(rt);
      } 
      _context.SaveChanges();

      return tokenHandler.WriteToken(token);
    }

    // ================================================================
  // public RefreshToken GenerateRefreshToken(string ipAddress)
  //  {
  //    var refreshToken = new RefreshToken
  //    {
  //      Token = getUniqueToken(),
  //      // token is valid for 7 days
  //      Expires = DateTime.UtcNow.AddDays(7),
  //      Created = DateTime.UtcNow,
  //      CreatedByIp = ipAddress
  //    };

  //    return refreshToken;

  //    string getUniqueToken()
  //    {
  //      // token is a cryptographically strong random sequence of values
  //      var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
  //      // ensure token is unique by checking against db
  //      var tokenIsUnique = !_context.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));

  //      if (!tokenIsUnique)
  //        return getUniqueToken();
            
  //      return token;
  //    }
  //}
    // ================================================================
  }
}
