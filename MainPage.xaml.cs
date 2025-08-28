namespace CredibillMauiApp;

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var user = await Microsoft.Maui.Storage.SecureStorage.GetAsync("LoggedUser");
            if (!string.IsNullOrEmpty(user))
            {
                WelcomeLabel.Text = $"Bienvenue {user}!";
            }
            else
            {
                WelcomeLabel.Text = "Bienvenue !";
            }
        }
        private async void GoToOtherPage_Clicked(object sender, EventArgs e)
        {
            // On utilise le nom enregistré pour la route
            await Shell.Current.GoToAsync(nameof(OtherPage));
        }
    }