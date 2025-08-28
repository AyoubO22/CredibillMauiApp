using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.ViewModels.Payments;

public partial class PaymentEditViewModel : BaseViewModel
{
    // Dummy data store
    public static List<Payment> Payments { get; set; } = new List<Payment>
    {
    new Payment { Id = 1, InvoiceId = 1, Amount = 50, DatePaid = DateTime.Today },
    new Payment { Id = 2, InvoiceId = 2, Amount = 75, DatePaid = DateTime.Today.AddDays(-1) }
    };

    [ObservableProperty] public Payment? payment;

    public PaymentEditViewModel() {}

    [RelayCommand]
    public void Save()
    {
        if (Payment == null) return;
        IsBusy = true;
        if (Payment.Id == 0)
        {
            Payment.Id = Payments.Count > 0 ? Payments.Max(p => p.Id) + 1 : 1;
            Payments.Add(Payment);
        }
        else
        {
            var index = Payments.FindIndex(p => p.Id == Payment.Id);
            if (index >= 0)
                Payments[index] = Payment;
        }
        IsBusy = false;
    }
}