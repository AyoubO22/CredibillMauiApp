using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Customers;

public partial class CustomersListViewModel : BaseViewModel
{
    private readonly CustomerService _service;

    public RelayCommand AddCommand { get; }

    public CustomersListViewModel(CustomerService service)
    {
        _service = service;
        AddCommand = new RelayCommand(async () => await Shell.Current.GoToAsync("customeredit"));
    }

    [ObservableProperty]
    ObservableCollection<Customer> customers = new();

    [ObservableProperty]
    string searchText = string.Empty;

    public ObservableCollection<Customer> FilteredCustomers =>
        string.IsNullOrWhiteSpace(SearchText)
            ? Customers
            : new ObservableCollection<Customer>(Customers.Where(c =>
                (c.Name ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (c.Email ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase)));

    partial void OnSearchTextChanged(string value)
    {
        OnPropertyChanged(nameof(FilteredCustomers));
    }

    partial void OnCustomersChanged(ObservableCollection<Customer> value)
    {
        OnPropertyChanged(nameof(FilteredCustomers));
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var list = await _service.GetCustomersAsync();
            Customers = new ObservableCollection<Customer>(list);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
