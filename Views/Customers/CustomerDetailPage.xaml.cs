using Microsoft.Maui.Controls;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.Views.Customers;

[QueryProperty(nameof(CustomerId), "id")]
public partial class CustomerDetailPage : ContentPage
{
    public int CustomerId { get; set; } = 0;

    public CustomerDetailPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Customer? customer = null;
        try
        {
            var service = App.Services.GetService(typeof(DatabaseService)) as DatabaseService;
            if (service != null && CustomerId > 0)
            {
                customer = await service.Connection.FindAsync<Customer>(CustomerId);
            }
        }
        catch { /* ignore */ }
        BindingContext = (object)(customer ?? new Customer());
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"customeredit?id={CustomerId}");
    }
}
