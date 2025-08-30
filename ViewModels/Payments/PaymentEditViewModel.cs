using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Payments;

public partial class PaymentEditViewModel : BaseViewModel
{
    private readonly PaymentService _service;
    private readonly InvoiceService _invoiceService;

    [ObservableProperty] public Payment? payment;

    [ObservableProperty]
    public ObservableCollection<Invoice> invoices = new();

    [ObservableProperty]
    public Invoice? selectedInvoice;

    public PaymentEditViewModel(PaymentService service, InvoiceService invoiceService)
    {
        _service = service;
        _invoiceService = invoiceService;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (Payment == null) return;
        try
        {
            IsBusy = true;
            if (SelectedInvoice != null)
                Payment.InvoiceId = SelectedInvoice.Id;
            if (Payment.InvoiceId == 0)
                throw new Exception("Please select an invoice.");
            if (Payment.Id == 0)
            {
                await _service.AddPaymentAsync(Payment);
            }
            else
            {
                await _service.UpdatePaymentAsync(Payment);
            }
        }
        finally { IsBusy = false; }
    }

    public async Task LoadInvoicesAsync()
    {
        var list = await _invoiceService.GetInvoicesAsync();
        Invoices = new ObservableCollection<Invoice>(list);
        if (Payment != null && Payment.InvoiceId > 0)
            SelectedInvoice = Invoices.FirstOrDefault(i => i.Id == Payment.InvoiceId);
    }

    partial void OnSelectedInvoiceChanged(Invoice? value)
    {
        if (Payment != null && value != null)
            Payment.InvoiceId = value.Id;
    }
}
