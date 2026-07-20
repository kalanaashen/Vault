using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Media;

namespace PasswordWallet.Views;

public partial class EditWindow : Window
{

    private PasswordEntry _entry;

    public EditWindow()
    {
        InitializeComponent();

    }

    public void Load_Data(PasswordEntry entry)
    {
        _entry = entry;
        UsernameTextBox.Text = entry.Username;
        WebsiteTextBox.Text = entry.Website;
        PasswordTextBox.Text = entry.Password;

    }
    private   void ConfrimButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_entry == null)
        {
            return;
        }
        _entry.Website = WebsiteTextBox.Text?.Trim() ?? "";
        _entry.Username = UsernameTextBox.Text?.Trim() ?? "";
        _entry.Password = PasswordTextBox.Text ?? "";

        Close();

    }


}