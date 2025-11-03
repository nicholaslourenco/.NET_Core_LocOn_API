using System;
using System.Security.Claims;
using System.Text;
using LocOn.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace LocOn.Services
{
    public class TokenService
    {
        private readonly string _chaveSecreta;
        private readonly string _jwtIssuer;

        public TokenService(IConfiguration configuration)
        {
            _chaveSecreta = configuration["JwtSettings:Secret"]
                ?? throw new ArgumentNullException("JwtSettings:Secret", "A chave secreta JWT não foi configurada.");

            _jwtIssuer = configuration["JwtSettings:Issuer"]
                ?? throw new ArgumentNullException("JwtSettings:Issuer", "O Issuer JWT não foi configurado.");
        }

        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_chaveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtIssuer,
                Audience = _jwtIssuer,

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Login),
                    new Claim(ClaimTypes.Role, usuario.Tipo),
                    new Claim("id", usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}