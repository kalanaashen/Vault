using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Media;
using System.Collections.Generic;

namespace PasswordWallet.Views;

public partial class CreateAccountWindow : Window
{
    private List<User> UserList = new List<User>();
    private User newuser = new User();


    public CreateAccountWindow()
    {
        InitializeComponent();


    }

    private void CreateAccountButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        bool isFound = false;
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
        foreach (var user in UserList)
        {
            if (user.Username == username)
            {
                MessageTextBlock.Text = "User has been already Registered";
                isFound = true;
            }



        }
        if (!isFound)
        {
            newuser.Username = username;
            newuser.Password = password;
            UserList.Add(newuser);
            Console.WriteLine("Account Created Succefully");
            MessageTextBlock.Foreground = Brushes.GreenYellow;
            MessageTextBlock.Text="Account Created Successfully!";
        }
        


       

        UsernameTextBox.Clear();
        PasswordTextBox.Clear();


    }
}