using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Invoices;

public partial class InvoiceEditViewModel : BaseViewModel
{
    private readonly InvoiceService _service;
    private readonly CustomerService _customersService;

    [ObservableProperty] public Invoice? invoice;

    [ObservableProperty]
    public ObservableCollection<Customer> customers = new();

    [ObservableProperty]
    public Customer? selectedCustomer;

    public InvoiceEditViewModel(InvoiceService service, CustomerService customersService)
    {
        _service = service;
        _customersService = customersService;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (Invoice == null) return;
        try
        {
            IsBusy = true;
            if (SelectedCustomer != null)
                Invoice.CustomerId = SelectedCustomer.Id;
            if (Invoice.CustomerId == 0)
                throw new Exception("Please select a customer.");
            if (Invoice.Id == 0)
            {
                await _service.AddInvoiceAsync(Invoice);
            }
            else
            {
                await _service.UpdateInvoiceAsync(Invoice);
            }
        }
        finally { IsBusy = false; }
    }

    public async Task LoadCustomersAsync()
    {
        var list = await _customersService.GetCustomersAsync();
        Customers = new ObservableCollection<Customer>(list);
        if (Invoice != null && Invoice.CustomerId > 0)
            SelectedCustomer = Customers.FirstOrDefault(c => c.Id == Invoice.CustomerId);
    }

    partial void OnSelectedCustomerChanged(Customer? value)
    {
        if (Invoice != null && value != null)
            Invoice.CustomerId = value.Id;
    }
}
