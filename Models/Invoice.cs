using SQLite;
namespace CredibillMauiApp.Models;

public class Invoice
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    [NotNull]
    public int CustomerId { get; set; }

    public string? Reference { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateIssued { get; set; }
}
