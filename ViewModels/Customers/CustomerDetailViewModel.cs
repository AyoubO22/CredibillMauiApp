using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.ViewModels.Customers;

public partial class CustomerDetailViewModel : BaseViewModel
{
    private readonly IApiClient _api;

    [ObservableProperty] Customer? customer;

    public CustomerDetailViewModel(IApiClient api) => _api = api;

    [RelayCommand]
    public async Task LoadAsync(int id)
    {
        try
        {
            IsBusy = true;
            var result = await _api.GetCustomerAsync(id);
            if (result.Success)
                Customer = result.Data;
            else
                ErrorMessage = result.Error;
        }
        finally { IsBusy = false; }
    }
}