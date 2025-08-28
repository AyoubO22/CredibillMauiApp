using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.ViewModels.Invoices;

public partial class InvoiceDetailViewModel : BaseViewModel
{
    private readonly IApiClient _api;

    [ObservableProperty] Invoice? invoice;

    public InvoiceDetailViewModel(IApiClient api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync(int id)
    {
        try
        {
            IsBusy = true;
            var result = await _api.GetInvoiceAsync(id);
            if (result.Success) Invoice = result.Data;
            else ErrorMessage = result.Error;
        }
        finally { IsBusy = false; }
    }
}