using Microsoft.Maui.Controls;
using System;
using CredibillMauiApp.ViewModels.Payments;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.Views.Payments
{
    [QueryProperty(nameof(Reference), "ref")]
    public partial class PaymentEditPage : ContentPage
    {
        public string Reference { get; set; } = string.Empty;

        public PaymentEditPage()
        {
            InitializeComponent();
            BindingContext = App.Services?.GetService(typeof(PaymentEditViewModel)) as PaymentEditViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PaymentEditViewModel vm)
            {
                Payment? existing = null;
                try
                {
                    var service = App.Services.GetService(typeof(PaymentService)) as PaymentService;
                    if (service != null && !string.IsNullOrWhiteSpace(Reference))
                    {
                        var list = await service.GetPaymentsAsync();
                        existing = list.FirstOrDefault(p => string.Equals(p.Reference, Reference, StringComparison.OrdinalIgnoreCase));
                    }
                }
                catch { /* ignore */ }
                vm.Payment = existing ?? new Payment { Reference = Reference, Amount = 0, DatePaid = DateTime.Today };
                await vm.LoadInvoicesAsync();
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (BindingContext is PaymentEditViewModel vm)
            {
            if (vm.SelectedInvoice == null && (vm.Payment?.InvoiceId ?? 0) == 0)
            {
                await DisplayAlert("Payment", "Please select an invoice.", "OK");
                return;
            }
            await vm.SaveAsync();
        }
            await DisplayAlert("Success", "Changes have been saved!", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
