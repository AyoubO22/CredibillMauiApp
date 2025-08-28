using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace CredibillMauiApp.Services
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;
        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var result = await _apiClient.RegisterAsync(username, password);
            return result;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var tokens = await _apiClient.LoginAsync(username, password);
            if (tokens != null)
            {
                if (tokens.Jwt != null)
                    await SecureStorage.SetAsync("jwt", tokens.Jwt);
                if (tokens.RefreshToken != null)
                    await SecureStorage.SetAsync("refresh", tokens.RefreshToken);
                return true;
            }
            return false;
        }

        public async Task<string?> GetJwtAsync()
        {
            return await SecureStorage.GetAsync("jwt");
        }

        public async Task<bool> RefreshTokenAsync()
        {
            var refresh = await SecureStorage.GetAsync("refresh");
            if (string.IsNullOrEmpty(refresh)) return false;
            var tokens = await _apiClient.RefreshTokenAsync(refresh);
            if (tokens != null)
            {
                if (tokens.Jwt != null)
                    await SecureStorage.SetAsync("jwt", tokens.Jwt);
                if (tokens.RefreshToken != null)
                    await SecureStorage.SetAsync("refresh", tokens.RefreshToken);
                return true;
            }
            return false;
        }

        public async Task<string?> GetLoggedUserAsync()
        {
            // For demo/testing, return JWT as 'logged user' if present
            var jwt = await SecureStorage.GetAsync("jwt");
            return jwt;
        }

        public void Logout()
        {
            SecureStorage.Remove("jwt");
            SecureStorage.Remove("refresh");
        }
    }
}
