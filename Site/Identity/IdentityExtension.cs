using System.Security.Claims;
using System.Security.Principal;

namespace Site.Identity
{
    public static class IdentityExtension
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

        public static class CustomClaimTypes
        {
            public const string Id = "Id";
            public const string Nome = "Nome";
        }
    }
}
