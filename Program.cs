var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connectionString = "Data Source=SafeVault.db";
var db = new sqlCommands(connectionString);
db.EnsureDatabase();


//to handle password and role validation
var auth = new AuthService(db);

// to check for bugs in authorization
var authS = new AuthorizationService();



// Register a new user
app.MapPost("/register", (Registration user_reg) =>
{
    auth.Register(user_reg.Username, user_reg.Password, user_reg.Role);
    return Results.Ok("User registered successfully.");
});



// Login
app.MapPost("/login", (Login user_login) =>
{
    var success = auth.Login(user_login.Username, user_login.Password);
    return success ? Results.Ok("Login successful.") : Results.Unauthorized();
});



// Admin-only route
app.MapGet("/admin", (string username) =>
{
    var user = db.GetUserWithPassword(username);
    if (user == null)
        return Results.Unauthorized();

    var role = user["Role"]?.ToString() ?? string.Empty;
    return authS.IsAuthorized(role, "admin")
        ? Results.Ok("Welcome Admin — you have full access.")
        : Results.Forbid();
});



// User route
app.MapGet("/user", (string username) =>
{
    var user = db.GetUserWithPassword(username);
    if (user == null)
        return Results.Unauthorized();

    // encode username before echoing it back
    var safeUsername = ValidateInput.EncodeForHtml(username);

    return Results.Ok($"Hello {safeUsername}, you have user access.");
});



app.Run();


record Registration(string Username, string Password, string Role);
record Login(string Username, string Password);