using Microsoft.Maui.Controls;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnCreateClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text ?? string.Empty;
            var email = EmailEntry.Text ?? string.Empty;
            var password = PasswordEntry.Text ?? string.Empty;
            var confirm = ConfirmEntry.Text ?? string.Empty;
            // Validate password rules
            if (password != confirm)
            {
                await DisplayAlert("Sign up", "Passwords do not match.", "OK");
                return;
            }
            if (!IsStrongPassword(password))
            {
                await DisplayAlert("Sign up", "Password must be at least 8 chars with 1 uppercase letter, 1 number and 1 special character.", "OK");
                return;
            }
            var auth = App.Services.GetService(typeof(AuthService)) as AuthService;
            if (auth == null) { await DisplayAlert("Error", "Auth service not available.", "OK"); return; }

            var (ok, error) = await auth.RegisterLocalAsync(name, email, password);
            if (!ok)
            {
                await DisplayAlert("Sign up", error ?? "Failed to create account.", "OK");
                return;
            }

            await DisplayAlert("Success", "Account created and signed in.", "OK");
            if (Application.Current?.MainPage is AppShell shell)
                shell.FlyoutBehavior = FlyoutBehavior.Flyout;
            await Shell.Current.GoToAsync("///main");
        }

        private bool IsStrongPassword(string pwd)
        {
            if (string.IsNullOrEmpty(pwd) || pwd.Length < 8) return false;
            bool hasUpper = pwd.Any(char.IsUpper);
            bool hasDigit = pwd.Any(char.IsDigit);
            bool hasSpecial = pwd.Any(ch => !char.IsLetterOrDigit(ch));
            return hasUpper && hasDigit && hasSpecial;
        }

        private void ToggleEntryPassword(Entry entry, Button button)
        {
            entry.IsPassword = !entry.IsPassword;
            button.Text = entry.IsPassword ? "Show" : "Hide";
        }

        private void OnTogglePasswordClicked(object sender, EventArgs e) => ToggleEntryPassword(PasswordEntry, TogglePasswordBtn);
        private void OnToggleConfirmClicked(object sender, EventArgs e) => ToggleEntryPassword(ConfirmEntry, ToggleConfirmBtn);

        // Default Shell back button is shown (navigated via registered route).
    }
}
