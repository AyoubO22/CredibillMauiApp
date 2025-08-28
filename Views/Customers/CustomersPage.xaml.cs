
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using CredibillMauiApp.ViewModels.Customers;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.Views.Customers;

public partial class CustomersPage : ContentPage
{
    public CustomersPage()
    {
    InitializeComponent();
    BindingContext = new CustomersListViewModel(App.Services.GetService<IApiClient>());
    }

    private async void OnCustomerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        var selectedCustomer = e.CurrentSelection[0];
        await Shell.Current.GoToAsync($"customerdetails?name={((dynamic)selectedCustomer).Name}");
    }
}