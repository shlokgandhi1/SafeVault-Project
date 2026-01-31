using System.Data;
using Microsoft.Data.Sqlite;

public class sqlCommands
{
    private readonly string _connectionString;

    public sqlCommands(string connectionString)
    {
        _connectionString = connectionString 
            ?? throw new ArgumentNullException(nameof(connectionString));
    }


    // Create the Users table if it doesn't exist
    public void EnsureDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                PasswordHash TEXT NOT NULL,
                Email TEXT,
                Role TEXT NOT NULL
            );
        ";
        command.ExecuteNonQuery();
    }

    public void InsertUser(string rawUsername, string rawEmail)
    {
        var username = ValidateInput.ValidateUsername(rawUsername);
        var email = ValidateInput.ValidateEmail(rawEmail);

        using var connection = new SqliteConnection(_connectionString);
        using var command = connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO Users (Username, Email, PasswordHash, Role)
            VALUES ($Username, $Email, $PasswordHash, $Role);
        ";

        command.Parameters.AddWithValue("$Username", username);
        command.Parameters.AddWithValue("$Email", email);
        command.Parameters.AddWithValue("$PasswordHash", "N/A");
        command.Parameters.AddWithValue("$Role", "user");

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void InsertUserWithPassword(string username, string passwordHash, string role)
    {
        username = ValidateInput.ValidateUsername(username);

        using var connection = new SqliteConnection(_connectionString);
        using var command = connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO Users (Username, PasswordHash, Role)
            VALUES ($Username, $PasswordHash, $Role);
        ";

        command.Parameters.AddWithValue("$Username", username);
        command.Parameters.AddWithValue("$PasswordHash", passwordHash);
        command.Parameters.AddWithValue("$Role", role);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public DataRow? GetUserWithPassword(string username)
    {
        using var connection = new SqliteConnection(_connectionString);
        using var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT Username, PasswordHash, Role
            FROM Users
            WHERE Username = $Username
            LIMIT 1;
        ";

        command.Parameters.AddWithValue("$Username", username);

        connection.Open();

        using var reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        // Build a DataTable to hold the result
        var table = new DataTable();
        table.Columns.Add("Username");
        table.Columns.Add("PasswordHash");
        table.Columns.Add("Role");

        var row = table.NewRow();
        row["Username"] = reader.GetString(0);
        row["PasswordHash"] = reader.GetString(1);
        row["Role"] = reader.GetString(2);

        table.Rows.Add(row);

        return row;
    }
}