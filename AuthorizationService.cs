public class AuthorizationService
{
    public bool IsAuthorized(string role, string requiredRole)
    {
        if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(requiredRole))
            return false;

        return string.Equals(role, requiredRole, StringComparison.OrdinalIgnoreCase);
    }
}