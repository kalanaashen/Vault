using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace PasswordWallet.Models
{
    public static class CurrentUser
    {
        public static int Id { get; set; } = 0;
        public static string Username { get; set; } = string.Empty;
        public static byte[] EncryptionKey { get; set; } = Array.Empty<byte>();

        public static void Clear()
        {
            Id = 0;
            Username = string.Empty;
            Array.Clear(EncryptionKey, 0, EncryptionKey.Length);
            EncryptionKey = Array.Empty<byte>();
        }
    }
}
