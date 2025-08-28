using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Customers;
using CredibillMauiApp.Services;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Views.Customers;

[QueryProperty(nameof(Name), "name")]
public partial class CustomerEditPage : ContentPage
{
    public string Name { get; set; } = string.Empty;

    public CustomerEditPage()
    {
        Name = string.Empty;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new { Name = Name, Email = $"{Name?.ToLower().Replace(" ", ".")}@example.com" };
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is CustomerEditViewModel vm)
        {
            vm.Save();
        }
        await DisplayAlert("Succès", "Les modifications ont été sauvegardées !", "OK");
        await Shell.Current.GoToAsync(".."); // Retour au détail
    }
}