using System.Net.Http.Json;
using System.Text.Json;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient http) => _http = http;

    // Auth endpoints
    public async Task<bool> RegisterAsync(string username, string password)
    {
        var payload = new { Username = username, Password = password };
        var response = await _http.PostAsJsonAsync("api/auth/register", payload, _json);
        return response.IsSuccessStatusCode;
    }

    public class TokenResult { public string? Jwt { get; set; } public string? RefreshToken { get; set; } }

    public async Task<TokenResult?> LoginAsync(string username, string password)
    {
        var payload = new { Username = username, Password = password };
        var response = await _http.PostAsJsonAsync("api/auth/login", payload, _json);
        if (!response.IsSuccessStatusCode) return null;
        var tokens = await response.Content.ReadFromJsonAsync<TokenResult>(_json);
        return tokens;
    }

    public async Task<TokenResult?> RefreshTokenAsync(string refreshToken)
    {
        var payload = new { RefreshToken = refreshToken };
        var response = await _http.PostAsJsonAsync("api/auth/refresh", payload, _json);
        if (!response.IsSuccessStatusCode) return null;
        var tokens = await response.Content.ReadFromJsonAsync<TokenResult>(_json);
        return tokens;
    }
    // Removed duplicate fields and constructor

    public void SetToken(string? jwtToken)
    {
        if (jwtToken != null)
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        else
            _http.DefaultRequestHeaders.Authorization = null;
    }

    // Customers
    public async Task<ApiResult<List<Customer>>> GetCustomersAsync()
    {
        try { var res = await _http.GetFromJsonAsync<List<Customer>>("Customers", _json); return ApiResult<List<Customer>>.Ok(res!); }
        catch (Exception ex) { return ApiResult<List<Customer>>.Fail(ex.Message); }
    }

    public async Task<ApiResult<Customer>> GetCustomerAsync(int id)
    {
        try { var res = await _http.GetFromJsonAsync<Customer>($"Customers/{id}", _json); return ApiResult<Customer>.Ok(res!); }
        catch (Exception ex) { return ApiResult<Customer>.Fail(ex.Message); }
    }

    public async Task<ApiResult<Customer>> CreateCustomerAsync(Customer c)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("Customers", c, _json);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Customer>(_json);
                return ApiResult<Customer>.Ok(data!);
            }
            return ApiResult<Customer>.Fail("Erreur création");
        }
        catch (Exception ex) { return ApiResult<Customer>.Fail(ex.Message); }
    }

    public async Task<ApiResult<bool>> UpdateCustomerAsync(Customer c)
    {
        try { var res = await _http.PutAsJsonAsync($"Customers/{c.Id}", c, _json); return ApiResult<bool>.Ok(res.IsSuccessStatusCode); }
        catch (Exception ex) { return ApiResult<bool>.Fail(ex.Message); }
    }

    public async Task<ApiResult<bool>> DeleteCustomerAsync(int id)
    {
        try { var res = await _http.DeleteAsync($"Customers/{id}"); return ApiResult<bool>.Ok(res.IsSuccessStatusCode); }
        catch (Exception ex) { return ApiResult<bool>.Fail(ex.Message); }
    }

    // Invoices (similaire)
    public async Task<ApiResult<List<Invoice>>> GetInvoicesAsync()
    {
        try { var res = await _http.GetFromJsonAsync<List<Invoice>>("Invoices", _json); return ApiResult<List<Invoice>>.Ok(res!); }
        catch (Exception ex) { return ApiResult<List<Invoice>>.Fail(ex.Message); }
    }

    public async Task<ApiResult<Invoice>> GetInvoiceAsync(int id)
    {
        try { var res = await _http.GetFromJsonAsync<Invoice>($"Invoices/{id}", _json); return ApiResult<Invoice>.Ok(res!); }
        catch (Exception ex) { return ApiResult<Invoice>.Fail(ex.Message); }
    }

    public async Task<ApiResult<Invoice>> CreateInvoiceAsync(Invoice i)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("Invoices", i, _json);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Invoice>(_json);
                return ApiResult<Invoice>.Ok(data!);
            }
            return ApiResult<Invoice>.Fail("Erreur création");
        }
        catch (Exception ex) { return ApiResult<Invoice>.Fail(ex.Message); }
    }

    public async Task<ApiResult<bool>> UpdateInvoiceAsync(Invoice i)
    {
        try { var res = await _http.PutAsJsonAsync($"Invoices/{i.Id}", i, _json); return ApiResult<bool>.Ok(res.IsSuccessStatusCode); }
        catch (Exception ex) { return ApiResult<bool>.Fail(ex.Message); }
    }

    public async Task<ApiResult<bool>> DeleteInvoiceAsync(int id)
    {
        try { var res = await _http.DeleteAsync($"Invoices/{id}"); return ApiResult<bool>.Ok(res.IsSuccessStatusCode); }
        catch (Exception ex) { return ApiResult<bool>.Fail(ex.Message); }
    }

    // Payments (similaire)
    public async Task<ApiResult<List<Payment>>> GetPaymentsAsync()
    {
        try { var res = await _http.GetFromJsonAsync<List<Payment>>("Payments", _json); return ApiResult<List<Payment>>.Ok(res!); }
        catch (Exception ex) { return ApiResult<List<Payment>>.Fail(ex.Message); }
    }

    public async Task<ApiResult<Payment>> GetPaymentAsync(int id)
    {
        try { var res = await _http.GetFromJsonAsync<Payment>($"Payments/{id}", _json); return ApiResult<Payment>.Ok(res!); }
        catch (Exception ex) { return ApiResult<Payment>.Fail(ex.Message); }
    }

    public async Task<ApiResult<Payment>> CreatePaymentAsync(Payment p)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("Payments", p, _json);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Payment>(_json);
                return ApiResult<Payment>.Ok(data!);
            }
            return ApiResult<Payment>.Fail("Erreur création");
        }
        catch (Exception ex) { return ApiResult<Payment>.Fail(ex.Message); }
    }

    public async Task<ApiResult<bool>> UpdatePaymentAsync(Payment p)
    {
        try { var res = await _http.PutAsJsonAsync($"Payments/{p.Id}", p, _json); return ApiResult<bool>.Ok(res.IsSuccessStatusCode); }
        catch (Exception ex) { return ApiResult<bool>.Fail(ex.Message); }
    }

    public async Task<ApiResult<bool>> DeletePaymentAsync(int id)
    {
        try { var res = await _http.DeleteAsync($"Payments/{id}"); return ApiResult<bool>.Ok(res.IsSuccessStatusCode); }
        catch (Exception ex) { return ApiResult<bool>.Fail(ex.Message); }
    }
}