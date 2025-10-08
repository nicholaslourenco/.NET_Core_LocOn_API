using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using LocOn.Models;

namespace LocOn.Services
{
    public class TokenService
    {
        private readonly string _chaveSecreta;

        public TokenService(IConfiguration configuration) 
        {
            _chaveSecreta = configuration["JwtSettings:Secret"]; 
            
            if (string.IsNullOrEmpty(_chaveSecreta))
            {
                throw new ArgumentNullException("JwtSettings:Secret", "A chave secreta JWT não foi configurada.");
            }
        }

        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_chaveSecreta);

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Login),
                    new Claim(ClaimTypes.Role, usuario.Tipo),
                    new Claim("id", usuario.Id.ToString())
                }),
                Expires = System.DateTime.UtcNow.AddHours(2), // Validade do Token
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}