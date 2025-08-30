using SQLite;

namespace CredibillMauiApp.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Unique = true)]
    public string Name { get; set; } = string.Empty; // used to log in

    [Indexed(Unique = true)]
    public string Email { get; set; } = string.Empty;

    // Password storage
    public string PasswordSalt { get; set; } = string.Empty; // base64
    public string PasswordHash { get; set; } = string.Empty; // base64
}

