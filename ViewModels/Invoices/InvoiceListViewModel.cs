using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Invoices;

public partial class InvoicesListViewModel : BaseViewModel
{
    private readonly InvoiceService _service;
    private readonly CustomerService _customerService;

    public RelayCommand AddCommand { get; }

    [ObservableProperty] ObservableCollection<Invoice> invoices = new();

    [ObservableProperty] ObservableCollection<Customer> customers = new();

    [ObservableProperty] Customer? selectedCustomer;

    [ObservableProperty]
    string searchText = string.Empty;

    public ObservableCollection<Invoice> FilteredInvoices
        => new ObservableCollection<Invoice>(
            Invoices.Where(i =>
                // Customer filter (null or Id==0 means All)
                (SelectedCustomer == null || SelectedCustomer.Id == 0 || i.CustomerId == SelectedCustomer.Id)
                // Search filter: matches by invoice number (Id) or price (Amount), also accepts string contains
                && MatchesSearch(i)));

    private bool MatchesSearch(Invoice i)
    {
        if (string.IsNullOrWhiteSpace(SearchText)) return true;
        var query = SearchText.Trim();
        // numeric checks
        if (int.TryParse(query, out var idQuery) && i.Id == idQuery) return true;
        if (decimal.TryParse(query, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out var priceCur) && i.Amount == priceCur) return true;
        if (decimal.TryParse(query, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var priceInv) && i.Amount == priceInv) return true;
        // string contains fallbacks
        return (i.Reference ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
               || i.Id.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)
               || i.Amount.ToString().Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    partial void OnSearchTextChanged(string value)
    {
        OnPropertyChanged(nameof(FilteredInvoices));
    }

    partial void OnInvoicesChanged(ObservableCollection<Invoice> value)
    {
        OnPropertyChanged(nameof(FilteredInvoices));
    }

    partial void OnSelectedCustomerChanged(Customer? value)
    {
        OnPropertyChanged(nameof(FilteredInvoices));
    }

    public InvoicesListViewModel(InvoiceService service, CustomerService customerService)
    {
        _service = service;
        _customerService = customerService;
        AddCommand = new RelayCommand(async () => await Shell.Current.GoToAsync("invoiceedit"));
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var list = await _service.GetInvoicesAsync();
            Invoices = new ObservableCollection<Invoice>(list);

            var custs = await _customerService.GetCustomersAsync();
            var items = new List<Customer> { new Customer { Id = 0, Name = "All" } };
            items.AddRange(custs);
            Customers = new ObservableCollection<Customer>(items);
            if (SelectedCustomer == null)
                SelectedCustomer = Customers.FirstOrDefault();
        }
        finally { IsBusy = false; }
    }
}
