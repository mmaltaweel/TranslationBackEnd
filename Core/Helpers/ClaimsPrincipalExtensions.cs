namespace Core.Helpers;

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        // Fetch the user ID from the claims (using ClaimTypes.NameIdentifier)
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
