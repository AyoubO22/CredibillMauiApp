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
            var user = await CredibillMauiApp.Services.TokenStorage.GetAsync("LoggedUser");
            if (!string.IsNullOrEmpty(user))
            {
                WelcomeLabel.Text = $"Welcome to Credibill, {user}!";
            }
            else
            {
                WelcomeLabel.Text = "Welcome to Credibill";
            }
        }
        private async void GoToOtherPage_Clicked(object sender, EventArgs e)
        {
            // On utilise le nom enregistré pour la route
            await Shell.Current.GoToAsync(nameof(OtherPage));
        }
    }
