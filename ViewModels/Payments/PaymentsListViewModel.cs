using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;
using System.Collections.ObjectModel;

namespace CredibillMauiApp.ViewModels.Payments;

public partial class PaymentsListViewModel : BaseViewModel
{
    private readonly PaymentService _service;

    public RelayCommand AddCommand { get; }

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

    partial void OnPaymentsChanged(ObservableCollection<Payment> value)
    {
        OnPropertyChanged(nameof(FilteredPayments));
    }

    public PaymentsListViewModel(PaymentService service)
    {
        _service = service;
        AddCommand = new RelayCommand(async () => await Shell.Current.GoToAsync("paymentedit"));
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var list = await _service.GetPaymentsAsync();
            Payments = new ObservableCollection<Payment>(list);
        }
        finally { IsBusy = false; }
    }
}
