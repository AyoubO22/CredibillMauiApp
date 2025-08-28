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
            var authService = Application.Current?.Handler?.MauiContext?.Services.GetService(typeof(AuthService)) as AuthService;
            if (authService == null)
                throw new System.Exception("AuthService not found. Check DI registration.");
            BindingContext = new LoginViewModel(authService);
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
                        await Shell.Current.GoToAsync($"///main?user={user}");
                    }
                }
            };
        }
    }
}
