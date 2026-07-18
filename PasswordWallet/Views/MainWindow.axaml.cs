using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PasswordWallet.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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

        SavedWebsiteTextBlock.Text = $"Website: {website}";
        SavedUsernameTextBlock.Text = $"Username: {username}";
        SavedPasswordTextBlock.Text = $"Password: {password}";

        MessageTextBlock.Text = "Password saved temporarily.";

        WebsiteTextBox.Clear();
        UsernameTextBox.Clear();
        PasswordTextBox.Clear();
    }
}