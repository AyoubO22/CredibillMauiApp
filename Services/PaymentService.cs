
using System.Net.Http.Json;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Services;

public class PaymentService
{
    private readonly HttpClient _http;
    private readonly DatabaseService _dbService;

    public PaymentService(HttpClient http, DatabaseService dbService)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://localhost:5001/api/"); // ⚠️ adapte l’URL
        _dbService = dbService;
    }

    // Get all payments (try API, fallback to local)
    public async Task<List<Payment>> GetPaymentsAsync()
    {
        try
        {
            var apiResult = await _http.GetFromJsonAsync<List<Payment>>("Payments");
            if (apiResult != null && apiResult.Count > 0)
            {
                await _dbService.Connection.DeleteAllAsync<Payment>();
                await _dbService.Connection.InsertAllAsync(apiResult);
                return apiResult;
            }
        }
        catch
        {
        }
        return await _dbService.Connection.Table<Payment>().ToListAsync();
    }

    // Get single payment
    public async Task<Payment?> GetPaymentAsync(int id)
    {
        try
        {
            var apiResult = await _http.GetFromJsonAsync<Payment>($"Payments/{id}");
            if (apiResult != null)
            {
                await _dbService.Connection.InsertOrReplaceAsync(apiResult);
                return apiResult;
            }
        }
        catch
        {
        }
        return await _dbService.Connection.FindAsync<Payment>(id);
    }

    // Add payment (local + remote)
    public async Task AddPaymentAsync(Payment payment)
    {
        await _dbService.Connection.InsertAsync(payment);
        try
        {
            var response = await _http.PostAsJsonAsync("Payments", payment);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Update payment (local + remote)
    public async Task UpdatePaymentAsync(Payment payment)
    {
        await _dbService.Connection.UpdateAsync(payment);
        try
        {
            var response = await _http.PutAsJsonAsync($"Payments/{payment.Id}", payment);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Delete payment (local + remote)
    public async Task DeletePaymentAsync(int id)
    {
        await _dbService.Connection.DeleteAsync<Payment>(id);
        try
        {
            var response = await _http.DeleteAsync($"Payments/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Sync local changes to API (call when online)
    public async Task SyncAsync()
    {
        var localPayments = await _dbService.Connection.Table<Payment>().ToListAsync();
        foreach (var payment in localPayments)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"Payments/{payment.Id}", payment);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
            }
        }
    }
}