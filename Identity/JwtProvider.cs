using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeczkaCore.Models;
using Microsoft.IdentityModel.Tokens;

namespace TeczkaCore.Identity
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.Role, user.Role.Name),
                //new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("dd-MM-yyyy")),
            };

            //if (!string.IsNullOrEmpty(user.Nationality))
            //{
            //    claims.Add(
            //        new Claim("Nationality", user.Nationality)
            //    );
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.JwtExpireMinutes);

            var token = new JwtSecurityToken(
                _jwtOptions.JwtIssuer,
                _jwtOptions.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
