using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.ViewModels.Invoices;

public partial class InvoiceEditViewModel : BaseViewModel
{
    // Dummy data store
    public static List<Invoice> Invoices { get; set; } = new List<Invoice>
    {
    new Invoice { Id = 1, CustomerId = 1, Amount = 100, DateIssued = DateTime.Today },
    new Invoice { Id = 2, CustomerId = 2, Amount = 200, DateIssued = DateTime.Today.AddDays(-1) }
    };

    [ObservableProperty] public Invoice? invoice;

    public InvoiceEditViewModel() {}

    [RelayCommand]
    public void Save()
    {
        if (Invoice == null) return;
        IsBusy = true;
        if (Invoice.Id == 0)
        {
            Invoice.Id = Invoices.Count > 0 ? Invoices.Max(i => i.Id) + 1 : 1;
            Invoices.Add(Invoice);
        }
        else
        {
            var index = Invoices.FindIndex(i => i.Id == Invoice.Id);
            if (index >= 0)
                Invoices[index] = Invoice;
        }
        IsBusy = false;
    }
}