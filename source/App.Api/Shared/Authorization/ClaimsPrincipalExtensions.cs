namespace App.Web.Shared.Authorization
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
                return userId;
            return null;
        }

        public static bool HasPermission(this ClaimsPrincipal user, string permission)
        {
            return user.Claims.Any(c => c.Type == "permissions" && c.Value == permission);
        }

        public static IEnumerable<string> GetPermissions(this ClaimsPrincipal user)
        {
            return user.Claims
                       .Where(c => c.Type == "permissions")
                       .Select(c => c.Value);
        }
    }
}
