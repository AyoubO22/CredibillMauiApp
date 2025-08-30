namespace CredibillMauiApp;

public partial class OtherPage : ContentPage
{
    public OtherPage()
    {
        InitializeComponent();
    }

    private async void GoBack_Clicked(object sender, EventArgs e)
{
    // Navigate back to previous page
    await Shell.Current.GoToAsync("..");
}
}
