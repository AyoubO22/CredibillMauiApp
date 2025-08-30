using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace CredibillMauiApp.Services
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;
        private readonly DatabaseService _db;
        public AuthService(ApiClient apiClient, DatabaseService db)
        {
            _apiClient = apiClient;
            _db = db;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var result = await _apiClient.RegisterAsync(username, password);
            return result;
        }

        // Local registration
        public async Task<(bool ok, string? error)> RegisterLocalAsync(string name, string email, string password)
        {
            name = (name ?? string.Empty).Trim();
            email = (email ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, "Name, email, and password are required.");

            var existingByName = await _db.Connection.Table<Models.User>().Where(u => u.Name == name).FirstOrDefaultAsync();
            if (existingByName != null) return (false, "Name already exists.");
            var existingByEmail = await _db.Connection.Table<Models.User>().Where(u => u.Email == email).FirstOrDefaultAsync();
            if (existingByEmail != null) return (false, "Email already exists.");

            var (salt, hash) = Security.HashPassword(password);
            var user = new Models.User { Name = name, Email = email, PasswordSalt = salt, PasswordHash = hash };
            await _db.Connection.InsertAsync(user);
            await TokenStorage.SetAsync("LoggedUser", name);
            await TokenStorage.SetAsync("jwt", $"demo-{name}");
            await TokenStorage.SetAsync("refresh", "demo-refresh");
            return (true, null);
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            // If API auth is enabled, try server login first
            if (Constants.UseApiAuth)
            {
                var tokens = await _apiClient.LoginAsync(username, password);
                if (tokens != null)
                {
                    if (tokens.Jwt != null)
                        await TokenStorage.SetAsync("jwt", tokens.Jwt);
                    if (tokens.RefreshToken != null)
                        await TokenStorage.SetAsync("refresh", tokens.RefreshToken);
                    await TokenStorage.SetAsync("LoggedUser", username);
                    return true;
                }
                return false;
            }

            // Offline: check in local Users table (by name or email)
            return await LoginLocalAsync(username, password);
        }

        private async Task<bool> LoginLocalAsync(string identifier, string password)
        {
            identifier = (identifier ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password)) return false;
            var user = await _db.Connection.Table<Models.User>()
                .Where(u => u.Name == identifier || u.Email == identifier)
                .FirstOrDefaultAsync();
            if (user == null) return false;
            var ok = Security.VerifyPassword(password, user.PasswordSalt, user.PasswordHash);
            if (!ok) return false;
            await TokenStorage.SetAsync("LoggedUser", user.Name);
            await TokenStorage.SetAsync("jwt", $"demo-{user.Name}");
            await TokenStorage.SetAsync("refresh", "demo-refresh");
            return true;
        }

        public async Task<string?> GetJwtAsync()
        {
            return await TokenStorage.GetAsync("jwt");
        }

        public async Task<bool> RefreshTokenAsync()
        {
            if (!Constants.UseApiAuth)
                return true; // No-op in offline mode

            var refresh = await TokenStorage.GetAsync("refresh");
            if (string.IsNullOrEmpty(refresh)) return false;
            var tokens = await _apiClient.RefreshTokenAsync(refresh);
            if (tokens != null)
            {
                if (tokens.Jwt != null)
                    await TokenStorage.SetAsync("jwt", tokens.Jwt);
                if (tokens.RefreshToken != null)
                    await TokenStorage.SetAsync("refresh", tokens.RefreshToken);
                return true;
            }
            return false;
        }

        public async Task<string?> GetLoggedUserAsync()
        {
            var user = await TokenStorage.GetAsync("LoggedUser");
            if (!string.IsNullOrEmpty(user)) return user;
            // Fallback: return JWT if username not stored
            return await TokenStorage.GetAsync("jwt");
        }

        public void Logout()
        {
            TokenStorage.Remove("jwt");
            TokenStorage.Remove("refresh");
            TokenStorage.Remove("LoggedUser");
        }
    }
}
