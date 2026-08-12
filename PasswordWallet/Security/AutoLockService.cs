using Avalonia.Threading;
using System;
using SessionSecurity = PasswordWallet.Models.Security;
namespace PasswordWallet.Security;

public sealed class AutoLockService : IDisposable
{
    private readonly SessionSecurity _security;

    private readonly DispatcherTimer _timer;
    private bool _isLocked;

    public event Action? LockRequired;

    public AutoLockService(SessionSecurity security)
    {
        _security = security;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _timer.Tick += Timer_Tick;
    }

    public void Start()
    {
        _isLocked = false;
        _security.ResetActivity();
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void ResetActivity()
    {
        if (!_isLocked)
        {
            _security.ResetActivity();
        }
    }

    private void Timer_Tick(
        object? sender,
        EventArgs e)
    {
        if (!_isLocked && _security.ShouldLock())
        {
            _isLocked = true;
            _timer.Stop();

            LockRequired?.Invoke();
        }
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Tick -= Timer_Tick;
    }
}
