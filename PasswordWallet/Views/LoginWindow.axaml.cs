using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Media;

namespace PasswordWallet.Views;

public partial class LoginWindow : Window
{
   

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

        if(username=="admin" && password=="admin"){
            Console.WriteLine("Login Successful");

            MainWindow mainwindow=new MainWindow();
            mainwindow.Show();
            Close();
        }else{
            Console.WriteLine("Login UnSuccssful");
            MessageTextBlock.Foreground=Brushes.Red;
            MessageTextBlock.Text="Username or Password Invaild";
        }
       

        var newUser=new User(username,password);


        UsernameTextBox.Clear();
        PasswordTextBox.Clear();

      
    }
}