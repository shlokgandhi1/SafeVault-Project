public class AuthService
{
    private readonly sqlCommands _db;

    public AuthService(sqlCommands db)
    {
        _db = db;
    }

    public void Register(string username, string password, string role)
    {
        var cleanUser = ValidateInput.ValidateUsername(username);

        // basic password and role validation
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is required.");

        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        _db.InsertUserWithPassword(cleanUser, hash, role);
    }

    public bool Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        var user = _db.GetUserWithPassword(username);

        if (user == null)
            return false;

        var storedHash = user["PasswordHash"]?.ToString() ?? string.Empty;

        if (string.IsNullOrEmpty(storedHash))
            return false;

        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
}