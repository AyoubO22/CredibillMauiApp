using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Payments;

public partial class PaymentsListViewModel : BaseViewModel
{
    private readonly IApiClient _api;


    [ObservableProperty] ObservableCollection<Payment> payments = new();

    [ObservableProperty]
    string searchText = string.Empty;

    public ObservableCollection<Payment> FilteredPayments =>
        string.IsNullOrWhiteSpace(SearchText)
            ? Payments
            : new ObservableCollection<Payment>(Payments.Where(p =>
                (p.Reference ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.InvoiceId.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.Amount.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)));

    partial void OnSearchTextChanged(string value)
    {
        OnPropertyChanged(nameof(FilteredPayments));
    }

    public PaymentsListViewModel(IApiClient api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var result = await _api.GetPaymentsAsync();
            if (result.Success) Payments = new ObservableCollection<Payment>(result.Data!);
            else ErrorMessage = result.Error;
        }
        finally { IsBusy = false; }
    }
}