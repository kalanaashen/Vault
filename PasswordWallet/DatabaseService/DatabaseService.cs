using Microsoft.Data.Sqlite;
using PasswordWallet.Models;
using PasswordWallet.Security;
using System;
using System.Collections.Generic;

namespace PasswordWallet.Database;

public class DatabaseService
{
    private const string ConnectionString =
        "Data Source=PasswordWallet.db";

    private static SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();

        return connection;
    }

    public void CreateDatabase()
    {
        using var connection = OpenConnection();
    }

    public void CreateTable()
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Passwords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Website TEXT NOT NULL,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL
            )";
        command.ExecuteNonQuery();

        EnsurePasswordsTableSchema(connection);
    }

    private static void EnsurePasswordsTableSchema(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA table_info(Passwords);";

        using var reader = command.ExecuteReader();
        var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        while (reader.Read())
        {
            columns.Add(reader.GetString(1));
        }

        if (columns.Contains("Service") && !columns.Contains("Website"))
        {
            using var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE Passwords RENAME COLUMN Service TO Website;";
            alterCommand.ExecuteNonQuery();
        }
    }

    public void CreateUserTable()
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL
            )";
        command.ExecuteNonQuery();
    }

    public List<PasswordEntry> GetAllPasswords(
        byte[] encryptionKey)
    {
        using var connection = OpenConnection();
        var entries = new List<PasswordEntry>();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Website, Username, Password
            FROM Passwords
            ORDER BY Id DESC";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string website = reader.GetString(1);
            string username = reader.GetString(2);
            string password = reader.GetString(3);

            if (EncryptionService.TryDecrypt(password, encryptionKey, out var decrypted))
            {
                password = decrypted;
            }

            entries.Add(new PasswordEntry
            {
                Id = id,
                Website = website,
                Username = username,
                Password = password
            });
        }

        return entries;
    }

    public void EncryptPlainTextPasswords(byte[] encryptionKey)
    {
        using var connection = OpenConnection();

        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = @"
            SELECT Id, Password
            FROM Passwords";

        using var reader = selectCommand.ExecuteReader();
        var updateRows = new List<(int Id, string Password)>();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string password = reader.GetString(1);
            if (!EncryptionService.TryDecrypt(password, encryptionKey, out _))
            {
                updateRows.Add((id, password));
            }
        }

        foreach (var row in updateRows)
        {
            string encryptedPassword = EncryptionService.Encrypt(row.Password, encryptionKey);

            using var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = @"
                UPDATE Passwords
                SET Password = @Password
                WHERE Id = @Id";
            updateCommand.Parameters.AddWithValue("@Password", encryptedPassword);
            updateCommand.Parameters.AddWithValue("@Id", row.Id);
            updateCommand.ExecuteNonQuery();
        }
    }

    public void InsertUser(User user, string password)
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (Username, Password)
            VALUES (@Username, @Password)";
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Password", EncryptionService.HashPassword(password));
        command.ExecuteNonQuery();
    }

    public void UpdateUserPassword(int userId, string password)
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Users
            SET Password = @Password
            WHERE Id = @Id";
        command.Parameters.AddWithValue("@Password", EncryptionService.HashPassword(password));
        command.Parameters.AddWithValue("@Id", userId);
        command.ExecuteNonQuery();
    }

    public (bool Success, User? user) GetUserByUsername(string username)
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Username, Password
            FROM Users
            WHERE Username = @Username";
        command.Parameters.AddWithValue("@Username", username);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            int id = reader.GetInt32(0);
            string userName = reader.GetString(1);
            string password = reader.GetString(2);

            User user = new User(userName, password)
            {
                Id = id
            };
            return (true, user);
        }

        return (false, null);
    }

    public bool DeletePassword(int id)
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM Passwords WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        return command.ExecuteNonQuery() == 1;
    }

    public int InsertPassword(PasswordEntry entry, byte[] encryptionKey)
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Passwords (Website, Username, Password)
            VALUES (@Website, @Username, @Password);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("@Website", entry.Website);
        command.Parameters.AddWithValue("@Username", entry.Username);
        command.Parameters.AddWithValue(
            "@Password",
            EncryptionService.Encrypt(entry.Password, encryptionKey));

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public bool UpdatePassword(PasswordEntry entry, byte[] encryptionKey)
    {
        using var connection = OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Passwords
            SET Website = @Website,
                Username = @Username,
                Password = @Password
            WHERE Id = @Id";
        command.Parameters.AddWithValue("@Website", entry.Website);
        command.Parameters.AddWithValue("@Username", entry.Username);
        command.Parameters.AddWithValue(
            "@Password",
            EncryptionService.Encrypt(entry.Password, encryptionKey));
        command.Parameters.AddWithValue("@Id", entry.Id);

        return command.ExecuteNonQuery() == 1;
    }
}
