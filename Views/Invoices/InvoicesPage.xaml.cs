using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Invoices;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Views.Invoices;

public partial class InvoicesPage : ContentPage
{
    public InvoicesPage()
    {
        InitializeComponent();
        BindingContext = App.Services?.GetService(typeof(InvoicesListViewModel)) as InvoicesListViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is InvoicesListViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnInvoiceSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        var selectedInvoice = e.CurrentSelection[0] as Invoice;
        if (selectedInvoice == null) return;
        if (sender is CollectionView cv) cv.SelectedItem = null;
        await Shell.Current.GoToAsync($"invoicedetails?id={selectedInvoice.Id}");
    }
}
