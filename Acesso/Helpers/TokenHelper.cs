using Acesso.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Acesso.Helpers
{
    public class TokenHelper
    {
        public static string GenerateToken(UsuarioAutenticadoDto user)
        {
            var key = Encoding.ASCII.GetBytes("9c7db6ea-2a05-48ac-99b7-b805f05138f0");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Usuario.Nome!),
                    new Claim("UserId", user.Usuario.Id.ToString()!),
                    new Claim("Login", user.Usuario.Email!),
                    new Claim("Acesso", user.Usuario.NivelAcesso.ToString()!)
                }),
                Expires = user.DataExpiracao,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }            
        }
    }
}
