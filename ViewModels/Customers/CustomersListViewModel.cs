using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Customers;

public partial class CustomersListViewModel : BaseViewModel
{
    private readonly IApiClient _api;

    public RelayCommand AddCommand { get; }

    public CustomersListViewModel(IApiClient api)
    {
        _api = api;
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

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var result = await _api.GetCustomersAsync();
            if (result.Success && result.Data != null && result.Data.Count > 0)
                Customers = new ObservableCollection<Customer>(result.Data!);
            else
            {
                // fallback to local demo data
                Customers = new ObservableCollection<Customer>(CustomerEditViewModel.Customers);
            }
        }
        catch (Exception)
        {
            // fallback to local demo data
            Customers = new ObservableCollection<Customer>(CustomerEditViewModel.Customers);
        }
        finally
        {
            IsBusy = false;
        }
    }
}