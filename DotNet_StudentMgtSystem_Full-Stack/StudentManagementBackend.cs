using Microsoft.Data.Sqlite;

public sealed class StudentDbService
{
    private readonly string _connectionString;

    public StudentDbService()
    {
        var databasePath = Path.Combine(AppContext.BaseDirectory, "students.db");
        _connectionString = $"Data Source={databasePath}";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var studentTable = connection.CreateCommand();
        studentTable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                Age INTEGER NOT NULL
            );";
        studentTable.ExecuteNonQuery();

        using var usersTable = connection.CreateCommand();
        usersTable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL,
                AccessCode TEXT
            );";
        usersTable.ExecuteNonQuery();
    }

    public List<Student> GetAll()
    {
        var students = new List<Student>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Email, Age FROM Students ORDER BY Id";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3)
            });
        }

        return students;
    }

    public Student? GetById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Email, Age FROM Students WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Student
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            Age = reader.GetInt32(3)
        };
    }

    public Student Create(Student student)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Students (Name, Email, Age)
            VALUES (@name, @email, @age);";

        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@email", student.Email);
        command.Parameters.AddWithValue("@age", student.Age);

        command.ExecuteNonQuery();

        using var idCommand = connection.CreateCommand();
        idCommand.CommandText = "SELECT last_insert_rowid();";
        student.Id = Convert.ToInt32(idCommand.ExecuteScalar());
        return student;
    }

    public bool Update(Student student)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Students
            SET Name = @name, Email = @email, Age = @age
            WHERE Id = @id;";

        command.Parameters.AddWithValue("@id", student.Id);
        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@email", student.Email);
        command.Parameters.AddWithValue("@age", student.Age);

        return command.ExecuteNonQuery() > 0;
    }

    public bool Delete(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Students WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        var deleted = command.ExecuteNonQuery() > 0;

        if (deleted)
        {
            ResetAutoIncrement(connection);
        }

        return deleted;
    }

    private static void ResetAutoIncrement(SqliteConnection connection)
    {
        using var countCommand = connection.CreateCommand();
        countCommand.CommandText = "SELECT COUNT(*) FROM Students";
        var remainingCount = Convert.ToInt32(countCommand.ExecuteScalar());

        if (remainingCount == 0)
        {
            using var resetCommand = connection.CreateCommand();
            resetCommand.CommandText = "DELETE FROM sqlite_sequence WHERE name = 'Students'";
            resetCommand.ExecuteNonQuery();
        }
    }

    public bool SignUp(UserAccount user)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @username";
        checkCommand.Parameters.AddWithValue("@username", user.Username);

        if (Convert.ToInt32(checkCommand.ExecuteScalar()) > 0)
        {
            return false;
        }

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (Username, Password, Role, AccessCode)
            VALUES (@username, @password, @role, @accessCode);";

        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@role", user.Role);
        command.Parameters.AddWithValue("@accessCode", user.AccessCode);

        return command.ExecuteNonQuery() > 0;
    }

    public UserAccount? Login(string username, string password)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Username, Password, Role, AccessCode FROM Users WHERE Username = @username AND Password = @password";
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password", password);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new UserAccount
        {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            Role = reader.GetString(3),
            AccessCode = reader.IsDBNull(4) ? null : reader.GetString(4)
        };
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class UserAccount
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? AccessCode { get; set; }
}
