# SafeVault-Project


## How to Run
- Install dependencies:
```dotnet restore```

- Run the API:
```dotnet run```

- The server will start at:
```http://localhost:5007```

- Use the ```Requests.http``` file to send requests


## Requirements
- .NET 10+
- SQLite
- REST Client extension (optional)


## Features
- Secure password hashing with bcrypt
- Parameterized SQLite queries to prevent SQL injection
- Input validation for usernames, emails, and roles
- HTML encoding to prevent XSS
- Role‑based authorization (admin vs. user)
- Automatic database initialization
