using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Media;
using System.Collections.Generic;
using PasswordWallet.Database;

namespace PasswordWallet.Views;

public partial class CreateAccountWindow : Window
{
    
    private User newuser = new User();
    private readonly DatabaseService database = new DatabaseService();


    public CreateAccountWindow()
    {
        InitializeComponent();


    }

    private void CreateAccountButton_Click(
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
   

        if (CheckExists(username))
        {
            MessageTextBlock.Foreground = Brushes.Red;
            MessageTextBlock.Text = "Username already exists. Please choose a different username.";
            return;
        }
            newuser.Username = username;
            newuser.Password = string.Empty;
            
            Console.WriteLine("Account Created Succefully");
            database.InsertUser(newuser, password);
            MessageTextBlock.Foreground = Brushes.GreenYellow;
            MessageTextBlock.Text = "Account Created Successfully!";
            UsernameTextBox.Clear();
            PasswordTextBox.Clear();



    }


    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {

        var loginWindow = new LoginWindow();

        loginWindow.Show();
        Close();


    }




    private bool CheckExists( String username)
    {
        
        
        var (success, user) = database.GetUserByUsername(username);
    

        return success;
    }

}
