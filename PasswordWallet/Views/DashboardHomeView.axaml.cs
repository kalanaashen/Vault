using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PasswordWallet.Views;

public partial class DashboardHomeView : UserControl
{
    // Events to communicate with DashboardWindow
    public event EventHandler? OpenPasswordManagerRequested;
    public event EventHandler? OpenPasswordGeneratorRequested;

    public event EventHandler? OpenSettingWindowRequested;
    public DashboardHomeView()
    {
        InitializeComponent();
    }

    private void OpenPasswordManager_Click(object? sender, RoutedEventArgs e)
    {
        OpenPasswordManagerRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OpenPasswordGenerator_Click(object? sender, RoutedEventArgs e)
    {
        OpenPasswordGeneratorRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OpenSettingButton_Click(object? sender, RoutedEventArgs e)
    {

        OpenSettingWindowRequested?.Invoke(this, EventArgs.Empty);



    }
}