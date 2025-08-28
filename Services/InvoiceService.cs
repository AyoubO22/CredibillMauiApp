
using System.Net.Http.Json;
using CredibillMauiApp.Models;

namespace CredibillMauiApp.Services;

public class InvoiceService
{
    private readonly HttpClient _http;
    private readonly DatabaseService _dbService;

    public InvoiceService(HttpClient http, DatabaseService dbService)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://localhost:5001/api/"); // ⚠️ adapte l’URL
        _dbService = dbService;
    }

    // Get all invoices (try API, fallback to local)
    public async Task<List<Invoice>> GetInvoicesAsync()
    {
        try
        {
            var apiResult = await _http.GetFromJsonAsync<List<Invoice>>("Invoices");
            if (apiResult != null && apiResult.Count > 0)
            {
                await _dbService.Connection.DeleteAllAsync<Invoice>();
                await _dbService.Connection.InsertAllAsync(apiResult);
                return apiResult;
            }
        }
        catch
        {
        }
        return await _dbService.Connection.Table<Invoice>().ToListAsync();
    }

    // Get single invoice
    public async Task<Invoice?> GetInvoiceAsync(int id)
    {
        try
        {
            var apiResult = await _http.GetFromJsonAsync<Invoice>($"Invoices/{id}");
            if (apiResult != null)
            {
                await _dbService.Connection.InsertOrReplaceAsync(apiResult);
                return apiResult;
            }
        }
        catch
        {
        }
        return await _dbService.Connection.FindAsync<Invoice>(id);
    }

    // Add invoice (local + remote)
    public async Task AddInvoiceAsync(Invoice invoice)
    {
        await _dbService.Connection.InsertAsync(invoice);
        try
        {
            var response = await _http.PostAsJsonAsync("Invoices", invoice);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Update invoice (local + remote)
    public async Task UpdateInvoiceAsync(Invoice invoice)
    {
        await _dbService.Connection.UpdateAsync(invoice);
        try
        {
            var response = await _http.PutAsJsonAsync($"Invoices/{invoice.Id}", invoice);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Delete invoice (local + remote)
    public async Task DeleteInvoiceAsync(int id)
    {
        await _dbService.Connection.DeleteAsync<Invoice>(id);
        try
        {
            var response = await _http.DeleteAsync($"Invoices/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Sync local changes to API (call when online)
    public async Task SyncAsync()
    {
        var localInvoices = await _dbService.Connection.Table<Invoice>().ToListAsync();
        foreach (var invoice in localInvoices)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"Invoices/{invoice.Id}", invoice);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
            }
        }
    }
}