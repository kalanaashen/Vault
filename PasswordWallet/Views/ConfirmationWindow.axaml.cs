using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PasswordWallet.Views;

public partial class ConfirmationWindow : Window
{
    public ConfirmationWindow(string message)
    {
        InitializeComponent();

        MessageTextBlock.Text = message;
    }

    private void ConfirmButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
