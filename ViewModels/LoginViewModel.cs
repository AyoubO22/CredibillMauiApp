using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CredibillMauiApp.Services;
using Microsoft.Maui.Controls;

namespace CredibillMauiApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private string _username;
        private string _password;
        private string _errorMessage;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public Command LoginCommand { get; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            var success = await _authService.LoginAsync(Username, Password);
            if (!success)
            {
                ErrorMessage = "Identifiants invalides.";
            }
            else
            {
                ErrorMessage = string.Empty;
                // Navigation handled in page
            }
        }

        public async Task<string?> GetLoggedUserAsync()
        {
            return await _authService.GetLoggedUserAsync();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
