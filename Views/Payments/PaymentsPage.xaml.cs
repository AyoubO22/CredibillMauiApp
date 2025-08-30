using Microsoft.Maui.Controls;
using CredibillMauiApp.ViewModels.Payments;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Views.Payments;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();
        BindingContext = App.Services?.GetService(typeof(PaymentsListViewModel)) as PaymentsListViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PaymentsListViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    private async void OnPaymentSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        var selectedPayment = e.CurrentSelection[0] as Payment;
        if (selectedPayment == null) return;
        if (sender is CollectionView cv) cv.SelectedItem = null;
        await Shell.Current.GoToAsync($"paymentdetails?ref={selectedPayment.Reference}");
    }
}
