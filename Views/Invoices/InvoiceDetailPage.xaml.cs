using Microsoft.Maui.Controls;

namespace CredibillMauiApp.Views.Invoices;

 [QueryProperty(nameof(InvoiceId), "id")]
public partial class InvoiceDetailPage : ContentPage
{
    public int InvoiceId { get; set; }
    = 0;

    public InvoiceDetailPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    BindingContext = new { InvoiceId = InvoiceId, Amount = "100 €" }; // mock
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
    await Shell.Current.GoToAsync($"invoiceedit?id={InvoiceId}");
    }
}