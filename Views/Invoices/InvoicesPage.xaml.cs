using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace CredibillMauiApp.Views.Invoices;

public partial class InvoicesPage : ContentPage
{
    public InvoicesPage()
    {
        InitializeComponent();
    // Data is now provided by the ViewModel via FilteredInvoices binding in XAML
    }

    private async void OnInvoiceSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

    var selectedInvoice = e.CurrentSelection[0];
    var invoiceId = ((dynamic)selectedInvoice).InvoiceId ?? ((dynamic)selectedInvoice).Id;
    await Shell.Current.GoToAsync($"invoicedetails?id={invoiceId}");
    }
}