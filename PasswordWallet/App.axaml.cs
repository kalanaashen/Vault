using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PasswordWallet.ViewModels;
using PasswordWallet.Views;
using PasswordWallet.Database;
namespace PasswordWallet;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()

    {
        DatabaseService database =
new DatabaseService();

        database.CreateDatabase();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            desktop.MainWindow = new LoginWindow
            {

            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
