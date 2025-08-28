using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Payments;
using CredibillMauiApp.Services;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Views.Payments;

[QueryProperty(nameof(Reference), "ref")]
public partial class PaymentEditPage : ContentPage
{
    public string Reference { get; set; } = string.Empty;

    public PaymentEditPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new { Reference = Reference, Amount = "50 €" };
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is PaymentEditViewModel vm)
        {
            vm.Save();
        }
        await DisplayAlert("Succès", "Les modifications ont été sauvegardées !", "OK");
        await Shell.Current.GoToAsync(".."); // retour au détail
    }
}