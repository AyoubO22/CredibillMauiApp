using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.ViewModels.Payments;

public partial class PaymentDetailViewModel : BaseViewModel
{
    private readonly IApiClient _api;

    [ObservableProperty] Payment? payment;

    public PaymentDetailViewModel(IApiClient api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync(int id)
    {
        try
        {
            IsBusy = true;
            var result = await _api.GetPaymentAsync(id);
            if (result.Success) Payment = result.Data;
            else ErrorMessage = result.Error;
        }
        finally { IsBusy = false; }
    }
}