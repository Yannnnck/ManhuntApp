// Datei: Manhunt.Backend/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Manhunt.Backend.Models.Requestsauth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Manhunt.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Dummy-Login: Akzeptiert im Body { "userId": "irgendwas", "username": "Alice" } 
        /// und gibt einen JWT-Token zurück, den der MAUI-Client später verwenden kann.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthLoginRequest dto)
        {
            // In echt: Hier gegen echten IdentityProvider oder DB prüfen. 
            // Wir nehmen an, dass dto.UserId & dto.Username gültig sind.
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expiryMin = jwtSettings.GetValue<int>("TokenLifetimeMinutes");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            // Claims: Sub = UserId, Name = Username
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, dto.UserId),
                new Claim(JwtRegisteredClaimNames.UniqueName, dto.Username),
                new Claim(ClaimTypes.NameIdentifier, dto.UserId),
                new Claim(ClaimTypes.Name, dto.Username)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(expiryMin),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { token = jwt });
        }
    }

    #region Hilfsklassen für Requests

    #endregion
}
