using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.ViewModels.Customers;

public partial class CustomerEditViewModel : BaseViewModel
{
    private readonly CustomerService _service;

    [ObservableProperty] public Customer? customer;

    public CustomerEditViewModel(CustomerService service)
    {
        _service = service;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (Customer == null) return;
        try
        {
            IsBusy = true;
            if (Customer.Id == 0)
            {
                await _service.AddCustomerAsync(Customer);
            }
            else
            {
                await _service.UpdateCustomerAsync(Customer);
            }
        }
        finally { IsBusy = false; }
    }
}
