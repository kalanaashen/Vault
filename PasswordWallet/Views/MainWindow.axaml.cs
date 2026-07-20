using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;

namespace PasswordWallet.Views;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<PasswordEntry> _passwordEntries;

    public MainWindow()
    {
        InitializeComponent();

        _passwordEntries = new ObservableCollection<PasswordEntry>();

        PasswordList.ItemsSource = _passwordEntries;
    }


    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {


        if (sender is Button button && button.CommandParameter is PasswordEntry entry)
        {

            bool isRemoved = _passwordEntries.Remove(entry);

            if (isRemoved)
            {
                MessageTextBlock.Text = "Record Deleted Successfully";
            }
            else
            {
                MessageTextBlock.Text = "Record Deletion UnSuccessfully";
            }


        }



    }

    private async void EditButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is PasswordEntry entry)
        {
            var dialog = new EditWindow();
            dialog.Load_Data(entry);
            await dialog.ShowDialog(this);
            int index = _passwordEntries.IndexOf(entry);

            _passwordEntries.RemoveAt(index);

            _passwordEntries.Insert(index, entry);
            MessageTextBlock.Text = "Record updated successfully.";


        }
    }


    private void SaveButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        string website = WebsiteTextBox.Text?.Trim() ?? "";
        string username = UsernameTextBox.Text?.Trim() ?? "";
        string password = PasswordTextBox.Text ?? "";

        if (string.IsNullOrWhiteSpace(website))
        {
            MessageTextBlock.Text = "Please enter a website.";
            return;
        }

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

        var newEntry = new PasswordEntry
        {
            Website = website,
            Username = username,
            Password = password
        };

        _passwordEntries.Add(newEntry);

        MessageTextBlock.Text = "Password added successfully.";

        WebsiteTextBox.Clear();
        UsernameTextBox.Clear();
        PasswordTextBox.Clear();

        WebsiteTextBox.Focus();
    }
}