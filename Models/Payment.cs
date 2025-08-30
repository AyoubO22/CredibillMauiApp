using SQLite;
namespace CredibillMauiApp.Models;

    public class Payment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        [NotNull]
        public int InvoiceId { get; set; }

        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
        public string? Reference { get; set; }
    }
