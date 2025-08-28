
using System.Net.Http.Json;
using CredibillMauiApp.Models;
using SQLite;

namespace CredibillMauiApp.Services;

public class CustomerService
{
    private readonly HttpClient _http;
    private readonly DatabaseService _dbService;

    public CustomerService(HttpClient http, DatabaseService dbService)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://localhost:5001/api/"); // ⚠️ adapte l’URL
        _dbService = dbService;
    }

    // Get all customers (try API, fallback to local)
    public async Task<List<Customer>> GetCustomersAsync()
    {
        try
        {
            var apiResult = await _http.GetFromJsonAsync<List<Customer>>("Customers");
            if (apiResult != null && apiResult.Count > 0)
            {
                // Sync local DB with API
                await _dbService.Connection.DeleteAllAsync<Customer>();
                await _dbService.Connection.InsertAllAsync(apiResult);
                return apiResult;
            }
        }
        catch
        {
            // API unavailable, use local
        }
        return await _dbService.Connection.Table<Customer>().ToListAsync();
    }

    // Get single customer
    public async Task<Customer?> GetCustomerAsync(int id)
    {
        try
        {
            var apiResult = await _http.GetFromJsonAsync<Customer>($"Customers/{id}");
            if (apiResult != null)
            {
                await _dbService.Connection.InsertOrReplaceAsync(apiResult);
                return apiResult;
            }
        }
        catch
        {
        }
        return await _dbService.Connection.FindAsync<Customer>(id);
    }

    // Add customer (local + remote)
    public async Task AddCustomerAsync(Customer customer)
    {
        await _dbService.Connection.InsertAsync(customer);
        try
        {
            var response = await _http.PostAsJsonAsync("Customers", customer);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            // Offline: will sync later
        }
    }

    // Update customer (local + remote)
    public async Task UpdateCustomerAsync(Customer customer)
    {
        await _dbService.Connection.UpdateAsync(customer);
        try
        {
            var response = await _http.PutAsJsonAsync($"Customers/{customer.Id}", customer);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Delete customer (local + remote)
    public async Task DeleteCustomerAsync(int id)
    {
        await _dbService.Connection.DeleteAsync<Customer>(id);
        try
        {
            var response = await _http.DeleteAsync($"Customers/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    // Sync local changes to API (call when online)
    public async Task SyncAsync()
    {
        var localCustomers = await _dbService.Connection.Table<Customer>().ToListAsync();
        foreach (var customer in localCustomers)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"Customers/{customer.Id}", customer);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
            }
        }
    }
}