using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace CredibillMauiApp.Views.Payments;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();
    // Data is now provided by the ViewModel via FilteredPayments binding in XAML
    }

    private async void OnPaymentSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

    var selectedPayment = e.CurrentSelection[0];
    var paymentRef = ((dynamic)selectedPayment).Reference ?? ((dynamic)selectedPayment).PaymentRef;
    await Shell.Current.GoToAsync($"paymentdetails?ref={paymentRef}");
    }
}