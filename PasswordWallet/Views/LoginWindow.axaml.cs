using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using PasswordWallet.Database;
using PasswordWallet.Models;
using PasswordWallet.Security;
using System;
using Avalonia.Media;

namespace PasswordWallet.Views;

public partial class LoginWindow : Window
{
    private readonly DatabaseService database = new DatabaseService();
    
    public LoginWindow()
    {
        InitializeComponent();


    }

    private void LoginButton_Click(
        object? sender,
        RoutedEventArgs e)
    {

        string username = UsernameTextBox.Text?.Trim() ?? "";
        string password = PasswordTextBox.Text ?? "";


        if (string.IsNullOrWhiteSpace(username))
        {
            MessageTextBlock.Text = "Please enter a username.";
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            MessageTextBlock.Text = "Please enter a password.";
            return;
        }
        User? user = null;
        bool success = false;
        (success, user) = database.GetUserByUsername(username);
        bool isValidPassword = success && user is not null &&
            (EncryptionService.VerifyPassword(password, user.Password) ||
             (!EncryptionService.IsPasswordHash(user.Password) &&
              string.Equals(user.Password, password, StringComparison.Ordinal)));

        if (isValidPassword && user is not null)
        {
            // Upgrade accounts created before password hashing was introduced.
            if (!EncryptionService.IsPasswordHash(user.Password))
            {
                database.UpdateUserPassword(user.Id, password);
            }

            CurrentUser.Id = user.Id;
            CurrentUser.Username = user.Username;
            CurrentUser.EncryptionKey = EncryptionService.DeriveKey(password);

            database.EncryptPlainTextPasswords(CurrentUser.EncryptionKey);

            var dashboardWindow = new DashboardWindow();
            dashboardWindow.Show();
            Close();
        }
        else
        {
            Console.WriteLine("Login UnSuccssful");
            MessageTextBlock.Foreground = Brushes.Red;
            MessageTextBlock.Text = "Username or Password Invaild";
        }





        UsernameTextBox.Clear();
        PasswordTextBox.Clear();


    }

    private void CreateAccountButton_Click(object? sender, RoutedEventArgs e)
    {

        var CreateAccountWindow = new CreateAccountWindow();
        CreateAccountWindow.Show();
        Close();

    }
}
