using Microsoft.Data.Sqlite;

namespace PasswordWallet.Database;

public class DatabaseService
{
    private const string ConnectionString =
        "Data Source=PasswordWallet.db";


    public void CreateDatabase()
    {
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();
    }

    public void CreateTable()
    {
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Passwords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Service TEXT NOT NULL,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL
            )";
        command.ExecuteNonQuery();
    }
    public void CreateUserTable()
    {
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL
            )";
        command.ExecuteNonQuery();
    }   
}

