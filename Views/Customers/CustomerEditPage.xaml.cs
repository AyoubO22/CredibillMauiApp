using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Customers;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.Views.Customers;

[QueryProperty(nameof(CustomerId), "id")]
public partial class CustomerEditPage : ContentPage
{
    public int CustomerId { get; set; } = 0;

    public CustomerEditPage()
    {
        InitializeComponent();
        BindingContext = App.Services?.GetService(typeof(CustomerEditViewModel)) as CustomerEditViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CustomerEditViewModel vm)
        {
            Customer? existing = null;
            try
            {
                var service = App.Services.GetService(typeof(CustomerService)) as CustomerService;
                if (service != null && CustomerId > 0)
                {
                    existing = await service.GetCustomerAsync(CustomerId);
                }
            }
            catch { /* ignore */ }

            vm.Customer = existing ?? new Customer();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is CustomerEditViewModel vm)
        {
            await vm.SaveAsync();
        }
        await DisplayAlert("Success", "Changes have been saved!", "OK");
        await Shell.Current.GoToAsync("..");
    }
}
