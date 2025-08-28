using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Invoices;
using CredibillMauiApp.Services;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Views.Invoices;

[QueryProperty(nameof(InvoiceId), "id")]
public partial class InvoiceEditPage : ContentPage
{
    public int InvoiceId { get; set; } = 0;

    public InvoiceEditPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    BindingContext = new { InvoiceId = InvoiceId, Amount = "100 €" };
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is InvoiceEditViewModel vm)
        {
            vm.Save();
        }
        await DisplayAlert("Succès", "Les modifications ont été sauvegardées !", "OK");
        await Shell.Current.GoToAsync(".."); // retour au détail
    }
}