using Application.Seguridad;
using System.Security.Claims;

namespace Application.Security
{
    public static class ClaimsExtensions
    {
        public static bool EsUsuarioRegistrado(this ClaimsPrincipal user)
        {
            return user?.Claims.Any(c =>
                c.Type == CustomClaims.UsuarioRegistrado &&
                c.Value == "true"
            ) == true;
        }
    }
}
