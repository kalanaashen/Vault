using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PasswordWallet.Views;

public partial class DashboardWindow : Window
{
    private readonly DashboardHomeView _homeView = new();

    private readonly MainWindow _passwordManagerView = new(); 
    private readonly PasswordGenWindow _generatorView = new();
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

   

    private void OpenPasswordManager()
    {
        
        MainContentArea.Content = _passwordManagerView;
    }

    private void OpenPasswordGenerator()
    {
      
        MainContentArea.Content = _generatorView;
    }

    private void Logout_Click(object? sender, RoutedEventArgs e)
    {

        var loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }
}