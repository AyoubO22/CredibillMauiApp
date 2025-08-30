using SQLite;
using SQLitePCL;
using System.IO;
using CredibillMauiApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CredibillMauiApp.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService()
        {
            // Ensure SQLite native bindings are loaded on all platforms
            Batteries_V2.Init();
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "credibill.db3");
            _db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitAsync()
        {
            try { await _db.ExecuteAsync("PRAGMA foreign_keys = ON;"); } catch { }
            await _db.CreateTableAsync<Customer>();
            await _db.CreateTableAsync<Invoice>();
            await _db.CreateTableAsync<Payment>();
            await _db.CreateTableAsync<User>();
        }

        public async Task SeedAsync()
        {
            if (await _db.Table<Customer>().CountAsync() == 0)
            {
                await _db.InsertAllAsync(new List<Customer>
                {
                    new Customer { Name = "Alice Martin", Email = "alice@test.com", Phone = "0600000001" },
                    new Customer { Name = "Bob Durand", Email = "bob@test.com", Phone = "0600000002" }
                });
            }
            if (await _db.Table<Invoice>().CountAsync() == 0)
            {
                await _db.InsertAllAsync(new List<Invoice>
                {
                    new Invoice { CustomerId = 1, Amount = 100, DateIssued = DateTime.Now },
                    new Invoice { CustomerId = 2, Amount = 200, DateIssued = DateTime.Now }
                });
            }
            if (await _db.Table<Payment>().CountAsync() == 0)
            {
                await _db.InsertAllAsync(new List<Payment>
                {
                    new Payment { InvoiceId = 1, Amount = 100, DatePaid = DateTime.Now, Reference = "PAY100" },
                    new Payment { InvoiceId = 2, Amount = 200, DatePaid = DateTime.Now, Reference = "PAY200" }
                });
            }
        }

        public SQLiteAsyncConnection Connection => _db;

        public async Task DeleteAllDataAsync()
        {
            // Delete in dependency order: Payments -> Invoices -> Customers -> Users
            await _db.DeleteAllAsync<Payment>();
            await _db.DeleteAllAsync<Invoice>();
            await _db.DeleteAllAsync<Customer>();
            await _db.DeleteAllAsync<User>();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _db.InsertOrReplaceAsync(customer);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _db.InsertOrReplaceAsync(invoice);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _db.InsertOrReplaceAsync(payment);
        }
    }
}
