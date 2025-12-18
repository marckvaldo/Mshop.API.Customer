using System.Security.Claims;
using System.Text.Json;

namespace MShop.API.Customer.Extension
{
    public static class ExtensionClaimsPrincipal
    {
        public static bool HasResourceRole(this ClaimsPrincipal user, string resource, string role)
        {
            var resourceAccessClaim =
                user.FindFirst("resource_access")?.Value;

            if (resourceAccessClaim is null)
                return false;

            using var doc = JsonDocument.Parse(resourceAccessClaim);

            if (!doc.RootElement.TryGetProperty(resource, out var resourceElement))
                return false;

            if (!resourceElement.TryGetProperty("roles", out var rolesElement))
                return false;

            return rolesElement.EnumerateArray()
                .Any(r => r.GetString() == role);
        }
    }
}
