using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PasswordWallet.Views;

public partial class DashboardWindow : Window
{
    public DashboardWindow()
    {
        InitializeComponent();
    }

    private async void OpenPasswordManager_Click(
        object? sender,
        RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();

        await mainWindow.ShowDialog(this);
    }

    private async void OpenPasswordGenerator_Click(
        object? sender,
        RoutedEventArgs e)
    {
        PasswordGenWindow passwordGenWindow =
            new PasswordGenWindow();

        await passwordGenWindow.ShowDialog(this);
    }

    private void Logout_Click(
        object? sender,
        RoutedEventArgs e)
    {
        LoginWindow loginWindow = new LoginWindow();

        loginWindow.Show();
        Close();
    }
}