
using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Customers;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Views.Customers;

public partial class CustomersPage : ContentPage
{
    public CustomersPage()
    {
        InitializeComponent();
        BindingContext = App.Services?.GetService(typeof(CustomersListViewModel)) as CustomersListViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CustomersListViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnCustomerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        var selectedCustomer = e.CurrentSelection[0] as Customer;
        if (selectedCustomer == null) return;
        // Deselect so tapping the same row later still triggers
        if (sender is CollectionView cv) cv.SelectedItem = null;
        if (selectedCustomer.Id > 0)
            await Shell.Current.GoToAsync($"customerdetails?id={selectedCustomer.Id}");
    }
}
