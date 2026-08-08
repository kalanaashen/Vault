using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PasswordWallet.Views;

public partial class DashboardWindow : Window
{
    private readonly DashboardHomeView _homeView = new();

    private readonly MainWindow _passwordManagerView = new();
    private readonly PasswordGenWindow _generatorView = new();

    private readonly SettingsWindow _settingsView = new();
    public DashboardWindow()
    {



        InitializeComponent();

        // 1. Listen for card button clicks coming from DashboardHomeView
        _homeView.OpenPasswordManagerRequested += (s, e) => OpenPasswordManager();
        _homeView.OpenPasswordGeneratorRequested += (s, e) => OpenPasswordGenerator();
        _homeView.OpenSettingWindowRequested += (s, e) => OpenSettings();

        // 2. Load the dashboard home view by default inside the content area
        MainContentArea.Content = _homeView;
    }

    // Sidebar: Dashboard Button
    private void OpenDashboardHome_Click(object? sender, RoutedEventArgs e)
    {
        MainContentArea.Content = _homeView;
    }

    // Sidebar & Card: Manage Passwords Button
    private void OpenPasswordManager_Click(object? sender, RoutedEventArgs e)
    {
        OpenPasswordManager();
    }

    // Sidebar & Card: Password Generator Button
    private void OpenPasswordGenerator_Click(object? sender, RoutedEventArgs e)
    {
        OpenPasswordGenerator();
    }



    private void OpenPasswordManager()
    {

        MainContentArea.Content = _passwordManagerView;
    }

    private void OpenPasswordGenerator()
    {

        MainContentArea.Content = _generatorView;
    }

    private async void Logout_Click(object? sender, RoutedEventArgs e)
    {

        var confirmationWindow = new ConfirmationWindow("Are you sure you want to logout?");
        var isConfirmed = await confirmationWindow.ShowDialog<bool?>(this);
        if (isConfirmed == true)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }

    private void OpenSettings()
    {

        MainContentArea.Content = _settingsView;
    }
}
