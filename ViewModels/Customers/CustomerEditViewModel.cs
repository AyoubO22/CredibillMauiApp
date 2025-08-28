using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CredibillMauiApp.Models;
using CredibillMauiApp.Services;

namespace CredibillMauiApp.ViewModels.Customers;

public partial class CustomerEditViewModel : BaseViewModel
{
    // Dummy data store
    public static List<Customer> Customers { get; set; } = new List<Customer>
    {
        new Customer { Id = 1, Name = "Alice", Email = "alice@email.com" },
        new Customer { Id = 2, Name = "Bob", Email = "bob@email.com" }
    };

    [ObservableProperty] public Customer? customer;

    public CustomerEditViewModel() {}

    [RelayCommand]
    public void Save()
    {
        if (Customer == null) return;
        IsBusy = true;
        if (Customer.Id == 0)
        {
            // Création
            Customer.Id = Customers.Count > 0 ? Customers.Max(c => c.Id) + 1 : 1;
            Customers.Add(Customer);
        }
        else
        {
            // Mise à jour
            var index = Customers.FindIndex(c => c.Id == Customer.Id);
            if (index >= 0)
                Customers[index] = Customer;
        }
        IsBusy = false;
    }
}