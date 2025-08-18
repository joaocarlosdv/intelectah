using Acesso.Dtos;
using Application.Enums;
using System.Security.Claims;

namespace Acesso.Helpers
{
    public static class UsuarioConectadoHelper
    {
        public static int GetId(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity != null)
            {
                var c = claimsIdentity.FindFirst("UserId");

                if (c != null)
                {
                    return int.Parse(c.Value);
                }
            }

            return 0;
        }

        public static string GetNome(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity != null)
            {
                var c = claimsIdentity.FindFirst("Nome");

                if (c != null)
                {
                    return c.Value;
                }
            }

            return "";
        }

        public static string GetNivelAcesso(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity != null)
            {
                var c = claimsIdentity.FindFirst("Acesso");

                if (c != null)
                {
                    return c.Value;
                }
            }

            return "";
        }

        public static string GetToken(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity != null && claimsIdentity.Claims.Count() > 0)
                return claimsIdentity.FindFirst("TokenApi").Value;
            return string.Empty;
        }

        public static ClaimsPrincipal DefinirIdentidadeUsuario(UsuarioAutenticadoDto autenticado,
                                                               bool manterConectado = true)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, autenticado.Usuario.Nome!),
                    new Claim("IdUsuario", autenticado.Usuario.Id.ToString()!),
                    new Claim("Nome", autenticado.Usuario.Nome!.ToString()),
                    new Claim("Login", autenticado.Usuario.Email!.ToString()),
                    new Claim("TokenApi", autenticado.Token!),
                    new Claim("ManterConectado", manterConectado.ToString()),
                    new Claim("Acesso", autenticado.Usuario.NivelAcesso.ToString())
            };

            var userIdentity = new ClaimsIdentity(claims, "loginIntelectahApp");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            return principal;
        }
    }
}
