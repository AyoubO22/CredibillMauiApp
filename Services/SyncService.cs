using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using CredibillMauiApp.Models;
using Microsoft.Maui.Networking;

namespace CredibillMauiApp.Services
{
    public class SyncService
    {
        private readonly ApiClient _apiClient;
        private readonly DatabaseService _dbService;

        public SyncService(ApiClient apiClient, DatabaseService dbService)
        {
            _apiClient = apiClient;
            _dbService = dbService;
        }

        public async Task SyncAllAsync()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                // Customers
                var customersResult = await _apiClient.GetCustomersAsync();
                if (customersResult.Success && customersResult.Data != null)
                {
                    foreach (var c in customersResult.Data)
                    {
                        await _dbService.AddCustomerAsync(c);
                    }
                }
                // Invoices
                var invoicesResult = await _apiClient.GetInvoicesAsync();
                if (invoicesResult.Success && invoicesResult.Data != null)
                {
                    foreach (var i in invoicesResult.Data)
                    {
                        await _dbService.AddInvoiceAsync(i);
                    }
                }
                // Payments
                var paymentsResult = await _apiClient.GetPaymentsAsync();
                if (paymentsResult.Success && paymentsResult.Data != null)
                {
                    foreach (var p in paymentsResult.Data)
                    {
                        await _dbService.AddPaymentAsync(p);
                    }
                }
            }
            // If offline, use local DB only
        }
    }
}
