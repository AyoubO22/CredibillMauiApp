using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Invoices;

public partial class InvoicesListViewModel : BaseViewModel
{
    private readonly IApiClient _api;


    [ObservableProperty] ObservableCollection<Invoice> invoices = new();

    [ObservableProperty]
    string searchText = string.Empty;

    public ObservableCollection<Invoice> FilteredInvoices =>
        string.IsNullOrWhiteSpace(SearchText)
            ? Invoices
            : new ObservableCollection<Invoice>(Invoices.Where(i =>
                (i.Reference ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                i.CustomerId.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                i.Amount.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)));

    partial void OnSearchTextChanged(string value)
    {
        OnPropertyChanged(nameof(FilteredInvoices));
    }

    public InvoicesListViewModel(IApiClient api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var result = await _api.GetInvoicesAsync();
            if (result.Success)
                Invoices = new ObservableCollection<Invoice>(result.Data!);
            else
                ErrorMessage = result.Error;
        }
        finally { IsBusy = false; }
    }
}