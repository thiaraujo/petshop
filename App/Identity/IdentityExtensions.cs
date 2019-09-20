using System.Security.Claims;
using System.Security.Principal;

namespace App.Identity
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

        public static string GetCpf(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(CustomClaimTypes.Cpf);

            return claim?.Value ?? string.Empty;
        }

        public static class CustomClaimTypes
        {
            public const string Id = "Id";
            public const string Nome = "Nome";
            public const string Cpf = "Cpf";
        }
    }
}
