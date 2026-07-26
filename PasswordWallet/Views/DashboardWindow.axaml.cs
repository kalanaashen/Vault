using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PasswordWallet.Views;

public partial class DashboardWindow : Window
{
    private readonly DashboardHomeView _homeView = new();

    public DashboardWindow()
    {
        InitializeComponent();

        // 1. Listen for card button clicks coming from DashboardHomeView
        _homeView.OpenPasswordManagerRequested += (s, e) => OpenPasswordManager();
        _homeView.OpenPasswordGeneratorRequested += (s, e) => OpenPasswordGenerator();

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

    // --- Helper Methods to Open Your Windows ---

    private void OpenPasswordManager()
    {
        // Opens your existing MainWindow file as a popup/new window
        var managerWindow = new MainWindow();
        managerWindow.Show();
    }

    private void OpenPasswordGenerator()
    {
        // Opens your existing PasswordGenWindow file
        var generatorWindow = new PasswordGenWindow();
        generatorWindow.Show();
    }

    private void Logout_Click(object? sender, RoutedEventArgs e)
    {
      
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }
}