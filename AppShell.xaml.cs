using Microsoft.Maui.Controls;
using CredibillMauiApp.Views.Customers;
using CredibillMauiApp.Views.Payments;
using CredibillMauiApp.Views.Invoices;

namespace CredibillMauiApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(CustomersPage), typeof(CustomersPage));
        Routing.RegisterRoute(nameof(PaymentsPage), typeof(PaymentsPage));
        Routing.RegisterRoute(nameof(InvoicesPage), typeof(InvoicesPage));
        // Customer detail/edit routes
        Routing.RegisterRoute("CustomerDetailPage", typeof(CredibillMauiApp.Views.Customers.CustomerDetailPage));
        Routing.RegisterRoute("CustomerEditPage", typeof(CredibillMauiApp.Views.Customers.CustomerEditPage));
        // Invoice detail/edit routes
        Routing.RegisterRoute("InvoiceDetailPage", typeof(CredibillMauiApp.Views.Invoices.InvoiceDetailPage));
        Routing.RegisterRoute("InvoiceEditPage", typeof(CredibillMauiApp.Views.Invoices.InvoiceEditPage));
        // Payment detail/edit routes
        Routing.RegisterRoute("PaymentDetailPage", typeof(CredibillMauiApp.Views.Payments.PaymentDetailPage));
        Routing.RegisterRoute("PaymentEditPage", typeof(CredibillMauiApp.Views.Payments.PaymentEditPage));
        // Aliases for easier navigation
        Routing.RegisterRoute("customerdetails", typeof(CredibillMauiApp.Views.Customers.CustomerDetailPage));
        Routing.RegisterRoute("customeredit", typeof(CredibillMauiApp.Views.Customers.CustomerEditPage));
        Routing.RegisterRoute("invoicedetails", typeof(CredibillMauiApp.Views.Invoices.InvoiceDetailPage));
        Routing.RegisterRoute("invoiceedit", typeof(CredibillMauiApp.Views.Invoices.InvoiceEditPage));
        Routing.RegisterRoute("paymentdetails", typeof(CredibillMauiApp.Views.Payments.PaymentDetailPage));
        Routing.RegisterRoute("paymentedit", typeof(CredibillMauiApp.Views.Payments.PaymentEditPage));
    }
}