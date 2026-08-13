using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Media;
using PasswordWallet.Database;

namespace PasswordWallet.Views;

public partial class EditWindow : Window
{

    private readonly DatabaseService _dataService = new DatabaseService();


    private readonly PasswordEntry _entry;

    public EditWindow(PasswordEntry entry)
    {
        InitializeComponent();
        _entry = entry;
        UsernameTextBox.Text = entry.Username;
        WebsiteTextBox.Text = entry.Website;
        PasswordTextBox.Text = entry.Password;


    }


    private void ConfrimButton_Click(object? sender, RoutedEventArgs e)
    {
        string website = WebsiteTextBox.Text?.Trim() ?? "";
        string username = UsernameTextBox.Text?.Trim() ?? "";
        string password = PasswordTextBox.Text ?? "";

        if (string.IsNullOrWhiteSpace(website) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        _entry.Website = website;
        _entry.Username = username;
        _entry.Password = password;

        if (_dataService.UpdatePassword(_entry, CurrentUser.EncryptionKey))
        {
            Close(true);
        }

    }


}
