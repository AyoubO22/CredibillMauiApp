namespace CredibillMauiApp.Services;

public class ApiOptions
{
    public string BaseUrl { get; set; } = Constants.BaseApiUrl;
    public string? JwtToken { get; set; }
}