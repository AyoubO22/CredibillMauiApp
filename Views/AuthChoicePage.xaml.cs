using Microsoft.Maui.Controls;

namespace CredibillMauiApp.Views
{
    public partial class AuthChoicePage : ContentPage
    {
        public AuthChoicePage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("login");
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("register");
        }
    }
}
