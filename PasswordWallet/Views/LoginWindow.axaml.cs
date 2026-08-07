using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Media;
using PasswordWallet.Database;
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
        User user = new User();
        bool success = false;
        (success, user) = database.GetUserByUsername(username);
        if (success && user.Password == password)
        {
            Console.WriteLine("Login Successful");

            DashboardWindow dashboardWindow =
            new DashboardWindow();

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