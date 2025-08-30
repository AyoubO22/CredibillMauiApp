using CredibillMauiApp.ViewModels;
using CredibillMauiApp.Services;
using Microsoft.Maui.Controls;

namespace CredibillMauiApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            // Resolve via global app service provider. Handler.MauiContext can be null at this stage.
            var authService = App.Services.GetService(typeof(AuthService)) as AuthService;
            BindingContext = new LoginViewModel(authService ?? throw new System.Exception("AuthService not found. Check DI registration."));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as LoginViewModel;
            if (vm == null) return;
            vm.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(vm.ErrorMessage) && string.IsNullOrEmpty(vm.ErrorMessage))
                {
                    var user = await vm.GetLoggedUserAsync();
                    if (!string.IsNullOrEmpty(user))
                    {
                        if (Application.Current?.MainPage is AppShell shell)
                            shell.FlyoutBehavior = FlyoutBehavior.Flyout;
                        await Shell.Current.GoToAsync($"///main?user={user}");
                    }
                }
            };
        }

        // Default Shell back button will appear since this page is navigated
        // to via a registered route (not a root ShellContent).

        private void OnToggleLoginPasswordClicked(object sender, EventArgs e)
        {
            LoginPasswordEntry.IsPassword = !LoginPasswordEntry.IsPassword;
            ToggleLoginPwdBtn.Text = LoginPasswordEntry.IsPassword ? "Show" : "Hide";
        }
    }
}
