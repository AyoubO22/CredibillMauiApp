using Microsoft.Maui.Controls;

namespace CredibillMauiApp.Views.Customers;

[QueryProperty(nameof(Name), "name")]
public partial class CustomerDetailPage : ContentPage
{
    public string Name { get; set; } = string.Empty;

    public CustomerDetailPage()
    {
        Name = string.Empty;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new { Name = Name, Email = $"{Name.ToLower().Replace(" ", ".")}@example.com" };
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"customeredit?name={Name}");
    }
}