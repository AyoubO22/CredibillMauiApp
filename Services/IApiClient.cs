using CredibillMauiApp.Models;

namespace CredibillMauiApp.Services;

public interface IApiClient
{
    // Customers
    Task<ApiResult<List<Customer>>> GetCustomersAsync();
    Task<ApiResult<Customer>> GetCustomerAsync(int id);
    Task<ApiResult<Customer>> CreateCustomerAsync(Customer c);
    Task<ApiResult<bool>> UpdateCustomerAsync(Customer c);
    Task<ApiResult<bool>> DeleteCustomerAsync(int id);

    // Invoices
    Task<ApiResult<List<Invoice>>> GetInvoicesAsync();
    Task<ApiResult<Invoice>> GetInvoiceAsync(int id);
    Task<ApiResult<Invoice>> CreateInvoiceAsync(Invoice i);
    Task<ApiResult<bool>> UpdateInvoiceAsync(Invoice i);
    Task<ApiResult<bool>> DeleteInvoiceAsync(int id);

    // Payments
    Task<ApiResult<List<Payment>>> GetPaymentsAsync();
    Task<ApiResult<Payment>> GetPaymentAsync(int id);
    Task<ApiResult<Payment>> CreatePaymentAsync(Payment p);
    Task<ApiResult<bool>> UpdatePaymentAsync(Payment p);
    Task<ApiResult<bool>> DeletePaymentAsync(int id);

    // Auth optionnel
    void SetToken(string? jwtToken);
}