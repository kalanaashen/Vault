using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PasswordWallet.Models;
using System;
using Avalonia.Controls.Documents;

namespace PasswordWallet.Views;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<PasswordEntry> _passwordEntries;
    private readonly ObservableCollection<PasswordEntry> _filteredEntries;


    public MainWindow()
    {
        InitializeComponent();

        _passwordEntries = new ObservableCollection<PasswordEntry>();
        _filteredEntries = new ObservableCollection<PasswordEntry>();

        PasswordList.ItemsSource = _filteredEntries;
    }


    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {


        if (sender is Button button && button.CommandParameter is PasswordEntry entry)
        {

            bool isRemoved = _passwordEntries.Remove(entry);
            _filteredEntries.Remove(entry);

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
    private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {

        ApplySearchFilter();



    }
    private void ShowPassword_Click(
    object? sender,
    RoutedEventArgs e)
{
    if (sender is Button button &&
        button.CommandParameter is PasswordEntry entry)
    {
        entry.IsPasswordVisible = !entry.IsPasswordVisible;
        
        
        ApplySearchFilter();
    }
}
    private void ApplySearchFilter()
    {
        string searchText = SearchTextBox.Text?.Trim() ?? "";
        Search(searchText);

    }
    private void Search(string text)
    {

        _filteredEntries.Clear();
        foreach (var entry in this._passwordEntries)

        {

            if (string.IsNullOrWhiteSpace(text))
            {
                _filteredEntries.Add(entry);
                continue;
            }

            bool websiteMatches =
                entry.Website.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase);

            bool usernameMatches =
                entry.Username.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase);

            if (websiteMatches || usernameMatches)
            {
                _filteredEntries.Add(entry);
            }

        }
    }
    private async void EditButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.CommandParameter is PasswordEntry entry)
        {
            var dialog = new EditWindow(entry);


            await dialog.ShowDialog(this);

            ApplySearchFilter();
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
        _filteredEntries.Add(newEntry);

        MessageTextBlock.Text = "Password added successfully.";

        WebsiteTextBox.Clear();
        UsernameTextBox.Clear();
        PasswordTextBox.Clear();

        WebsiteTextBox.Focus();
    }
}