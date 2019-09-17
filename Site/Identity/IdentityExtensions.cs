using System.Security.Claims;
using System.Security.Principal;

namespace Site.Identity
{
    public static class IdentityExtensions
    {
        public static int GetId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(CustomClaimTypes.Id);

            return claim != null && claim.Value != null ? int.Parse(claim.Value) : 0;
        }

        public static string GetNome(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(CustomClaimTypes.Nome);

            return claim?.Value ?? string.Empty;
        }

        public static bool GetVeterinario(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(CustomClaimTypes.Veterinario);

            return claim != null && claim.Value != null ? bool.Parse(claim.Value) : false;
        }

        public static bool GetAdministrador(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(CustomClaimTypes.Administrador);

            return claim != null && claim.Value != null ? bool.Parse(claim.Value) : false;
        }

        public static class CustomClaimTypes
        {
            public const string Id = "Id";
            public const string Nome = "Nome";
            public const string Veterinario = "Veterinario";
            public const string Administrador = "Administrador";
        }
    }
}
