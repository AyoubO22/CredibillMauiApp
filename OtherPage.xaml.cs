namespace CredibillMauiApp;

public partial class OtherPage : ContentPage
{
    public OtherPage()
    {
        InitializeComponent();
    }

    private async void GoBack_Clicked(object sender, EventArgs e)
{
    // Retour vers MainPage via la route enregistrée
    await Shell.Current.GoToAsync(".."); // remonte d’un niveau
}
}