using Microsoft.Data.Sqlite;
using PasswordWallet.Models;
using System;
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
    public void GetAllUsers()
    {
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Users";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string username = reader.GetString(1);
            string password = reader.GetString(2);

            Console.WriteLine($"Id: {id}, Username: {username}, Password: {password}");
        }
    }
    public void InsertUser(User user)
    {
        
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (Username, Password)
            VALUES (@Username, @Password)";
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.ExecuteNonQuery();  
    }

    public (bool Success,User? user) GetUserByUsername(string username)
    {
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Users WHERE Username = @Username";
        command.Parameters.AddWithValue("@Username", username);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            int id = reader.GetInt32(0);
            string userName = reader.GetString(1);
            string password = reader.GetString(2);

            User user = new User(userName, password);
            user.Id = id;
            return (true, user);

            
        }
        return (false, null);
    }
    public void DeletePassword(int id)
    {
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM Passwords WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }

    public void InsertPassword(PasswordEntry entry)
    {
        using var connection =
            new SqliteConnection(ConnectionString);

        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Passwords (Service, Username, Password)
            VALUES (@Service, @Username, @Password)";
        command.Parameters.AddWithValue("@Service", entry.Website);
        command.Parameters.AddWithValue("@Username", entry.Username);
        command.Parameters.AddWithValue("@Password", entry.Password);
        command.ExecuteNonQuery();
    }
}

