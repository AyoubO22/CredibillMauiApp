using Microsoft.Maui.Controls;

namespace CredibillMauiApp.Views.Payments;

[QueryProperty(nameof(Reference), "ref")]
public partial class PaymentDetailPage : ContentPage
{
    public string Reference { get; set; }

    public PaymentDetailPage()
    {
        Reference = string.Empty;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new { Reference = Reference, Amount = "50 €" };
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"paymentedit?ref={Reference}");
    }
}