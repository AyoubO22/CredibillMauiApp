using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace CredibillMauiApp.Services
{
    // Simple abstraction to avoid Keychain on Mac Catalyst (use Preferences instead)
    public static class TokenStorage
    {
        public static Task SetAsync(string key, string value)
        {
#if MACCATALYST
            Preferences.Set(key, value);
            return Task.CompletedTask;
#else
            return SecureStorage.SetAsync(key, value);
#endif
        }

        public static Task<string?> GetAsync(string key)
        {
#if MACCATALYST
            if (!Preferences.ContainsKey(key))
                return Task.FromResult<string?>(null);
            var value = Preferences.Get(key, string.Empty);
            return Task.FromResult<string?>(string.IsNullOrEmpty(value) ? null : value);
#else
            return SecureStorage.GetAsync(key);
#endif
        }

        public static void Remove(string key)
        {
#if MACCATALYST
            Preferences.Remove(key);
#else
            SecureStorage.Remove(key);
#endif
        }

        public static void RemoveAll()
        {
#if MACCATALYST
            Preferences.Clear();
#else
            SecureStorage.RemoveAll();
#endif
        }
    }
}
