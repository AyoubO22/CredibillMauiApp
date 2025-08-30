using SQLite;

namespace CredibillMauiApp.Models;

public class Customer
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
