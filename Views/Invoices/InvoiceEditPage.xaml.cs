using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Invoices;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.Views.Invoices;

[QueryProperty(nameof(InvoiceId), "id")]
public partial class InvoiceEditPage : ContentPage
{
    public int InvoiceId { get; set; } = 0;

    public InvoiceEditPage()
    {
        InitializeComponent();
        BindingContext = App.Services?.GetService(typeof(InvoiceEditViewModel)) as InvoiceEditViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is InvoiceEditViewModel vm)
        {
            Invoice? existing = null;
            try
            {
                var service = App.Services.GetService(typeof(InvoiceService)) as InvoiceService;
                if (service != null && InvoiceId > 0)
                {
                    existing = await service.GetInvoiceAsync(InvoiceId);
                }
            }
            catch { /* ignore */ }
            vm.Invoice = existing ?? new Invoice { Id = 0, Amount = 0, DateIssued = DateTime.Today };
            await vm.LoadCustomersAsync();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is InvoiceEditViewModel vm)
        {
            if (vm.SelectedCustomer == null && (vm.Invoice?.CustomerId ?? 0) == 0)
            {
                await DisplayAlert("Invoice", "Please select a customer.", "OK");
                return;
            }
            await vm.SaveAsync();
        }
        await DisplayAlert("Success", "Changes have been saved!", "OK");
        await Shell.Current.GoToAsync(".."); // retour au détail
    }
}
