using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordWallet.Security;

public static class EncryptionService
{
    private const int KeyLength = 32;
    private const int Iterations = 100_000;
    private const int PasswordHashIterations = 210_000;
    private const int PasswordSaltLength = 16;
    private const int PasswordHashLength = 32;
    private static readonly byte[] Salt = Encoding.UTF8.GetBytes("PasswordWalletEncryptionSalt");
    private const string EncryptedPrefix = "ENC:";
    private const string PasswordHashPrefix = "PBKDF2-SHA256:";

    // Login passwords must be one-way hashed, not reversibly encrypted.
    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(PasswordSaltLength);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            PasswordHashIterations,
            HashAlgorithmName.SHA256,
            PasswordHashLength);

        return $"{PasswordHashPrefix}{PasswordHashIterations}:" +
            $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public static bool VerifyPassword(string password, string storedPassword)
    {
        if (string.IsNullOrWhiteSpace(storedPassword) ||
            !storedPassword.StartsWith(PasswordHashPrefix, StringComparison.Ordinal))
        {
            return false;
        }

        string[] parts = storedPassword.Split(':');
        if (parts.Length != 4 ||
            !int.TryParse(parts[1], out int iterations) ||
            iterations <= 0)
        {
            return false;
        }

        try
        {
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] expectedHash = Convert.FromBase64String(parts[3]);
            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool IsPasswordHash(string value) =>
        value.StartsWith(PasswordHashPrefix, StringComparison.Ordinal);

    public static byte[] DeriveKey(string password)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            password,
            Salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeyLength);
    }

    public static string Encrypt(
        string plainText,
        byte[] key)
    {
        byte[] nonce = RandomNumberGenerator.GetBytes(12);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = new byte[plainBytes.Length];
        byte[] tag = new byte[16];

        using var aes = new AesGcm(key, 16);
        aes.Encrypt(
            nonce,
            plainBytes,
            cipherBytes,
            tag);

        byte[] result = new byte[
            nonce.Length +
            tag.Length +
            cipherBytes.Length];

        Buffer.BlockCopy(
            nonce,
            0,
            result,
            0,
            nonce.Length);

        Buffer.BlockCopy(
            tag,
            0,
            result,
            nonce.Length,
            tag.Length);

        Buffer.BlockCopy(
            cipherBytes,
            0,
            result,
            nonce.Length + tag.Length,
            cipherBytes.Length);

        return EncryptedPrefix + Convert.ToBase64String(result);
    }

    public static bool TryDecrypt(
        string encryptedText,
        byte[] key,
        out string plainText)
    {
        plainText = string.Empty;
        if (string.IsNullOrEmpty(encryptedText) ||
            !encryptedText.StartsWith(EncryptedPrefix, StringComparison.Ordinal))
        {
            return false;
        }

        try
        {
            byte[] data = Convert.FromBase64String(
                encryptedText[EncryptedPrefix.Length..]);

            if (data.Length < 12 + 16)
            {
                return false;
            }

            byte[] nonce = new byte[12];
            byte[] tag = new byte[16];
            int cipherLength =
                data.Length -
                nonce.Length -
                tag.Length;
            byte[] cipherBytes = new byte[cipherLength];
            byte[] plainBytes = new byte[cipherLength];

            Buffer.BlockCopy(
                data,
                0,
                nonce,
                0,
                nonce.Length);
            Buffer.BlockCopy(
                data,
                nonce.Length,
                tag,
                0,
                tag.Length);
            Buffer.BlockCopy(
                data,
                nonce.Length + tag.Length,
                cipherBytes,
                0,
                cipherLength);

            using var aes = new AesGcm(key, 16);
            aes.Decrypt(
                nonce,
                cipherBytes,
                tag,
                plainBytes);

            plainText = Encoding.UTF8.GetString(plainBytes);
            return true;
        }
        catch (CryptographicException)
        {
            return false;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static string Decrypt(
        string encryptedText,
        byte[] key)
    {
        if (TryDecrypt(encryptedText, key, out var plainText))
        {
            return plainText;
        }

        throw new CryptographicException(
            "The provided data could not be decrypted.");
    }
}
