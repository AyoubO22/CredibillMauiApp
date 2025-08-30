using Microsoft.Extensions.Logging;
using CredibillMauiApp.Services;
using CredibillMauiApp.ViewModels;
using CredibillMauiApp.ViewModels.Customers;
using CredibillMauiApp.ViewModels.Invoices;
using CredibillMauiApp.ViewModels.Payments;
using CredibillMauiApp.Views.Customers;
using CredibillMauiApp.Views.Invoices;
using CredibillMauiApp.Views.Payments;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace CredibillMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Injection de dépendances
        // Configure a base address for the shared HttpClient used by ApiClient
        builder.Services.AddSingleton(sp => new HttpClient
        {
            BaseAddress = new Uri(Constants.BaseApiUrl)
        });
        // Register one ApiClient instance and map IApiClient to the same instance
        builder.Services.AddSingleton<ApiClient>();
        builder.Services.AddSingleton<IApiClient>(sp => sp.GetRequiredService<ApiClient>());
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<SyncService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<PrivacyPolicyViewModel>();
        builder.Services.AddHttpClient<CustomerService>()
            .AddTypedClient((http, sp) => new CustomerService(http, sp.GetRequiredService<DatabaseService>()));
        builder.Services.AddHttpClient<InvoiceService>()
            .AddTypedClient((http, sp) => new InvoiceService(http, sp.GetRequiredService<DatabaseService>()));
        builder.Services.AddHttpClient<PaymentService>()
            .AddTypedClient((http, sp) => new PaymentService(http, sp.GetRequiredService<DatabaseService>()));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // --- Services --- (deduplicated above)

        // --- ViewModels ---
        builder.Services.AddTransient<CustomersListViewModel>();
        builder.Services.AddTransient<CustomerDetailViewModel>();
        builder.Services.AddTransient<CustomerEditViewModel>();

        builder.Services.AddTransient<InvoicesListViewModel>();
        builder.Services.AddTransient<InvoiceDetailViewModel>();
        builder.Services.AddTransient<InvoiceEditViewModel>();

        builder.Services.AddTransient<PaymentsListViewModel>();
        builder.Services.AddTransient<PaymentDetailViewModel>();
        builder.Services.AddTransient<PaymentEditViewModel>();

        // --- Pages ---
        builder.Services.AddTransient<CustomersPage>();
        builder.Services.AddTransient<CustomerDetailPage>();
        builder.Services.AddTransient<CustomerEditPage>();

        builder.Services.AddTransient<InvoicesPage>();
        builder.Services.AddTransient<InvoiceDetailPage>();
        builder.Services.AddTransient<InvoiceEditPage>();

        builder.Services.AddTransient<PaymentsPage>();
        builder.Services.AddTransient<PaymentDetailPage>();
        builder.Services.AddTransient<PaymentEditPage>();

        var app = builder.Build();
        // Expose DI container globally for pages/viewmodels that use App.Services
        App.Services = app.Services;
        // Initialize and seed local SQLite database
        var dbService = app.Services.GetService<DatabaseService>();
        Task.Run(async () => {
            if (dbService != null)
            {
                await dbService.InitAsync();
                await dbService.SeedAsync();
            }
        }).Wait();
        return app;
    }
}
