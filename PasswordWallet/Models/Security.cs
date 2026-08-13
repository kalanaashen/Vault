using System;

namespace PasswordWallet.Models;

public class Security
{
    public TimeSpan AutoLockDuration { get; set; } =
        TimeSpan.FromMinutes(5);

    public DateTime LastActivityTime { get; private set; } =
        DateTime.UtcNow;

    public void ResetActivity()
    {
        LastActivityTime = DateTime.UtcNow;
    }

    public bool ShouldLock()
    {
        return DateTime.UtcNow - LastActivityTime
               >= AutoLockDuration;
    }
}
