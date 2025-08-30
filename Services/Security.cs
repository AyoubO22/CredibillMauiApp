using System.Security.Cryptography;
using System.Text;

namespace CredibillMauiApp.Services
{
    public static class Security
    {
        public static (string salt, string hash) HashPassword(string password)
        {
            // PBKDF2 with random 16-byte salt
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            return (Convert.ToBase64String(saltBytes), Convert.ToBase64String(hashBytes));
        }

        public static bool VerifyPassword(string password, string saltBase64, string hashBase64)
        {
            try
            {
                byte[] saltBytes = Convert.FromBase64String(saltBase64);
                byte[] expectedHash = Convert.FromBase64String(hashBase64);
                using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
                byte[] actual = pbkdf2.GetBytes(32);
                return CryptographicOperations.FixedTimeEquals(actual, expectedHash);
            }
            catch { return false; }
        }
    }
}

