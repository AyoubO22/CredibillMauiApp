using System.Net;

namespace CredibillMauiApp.Services;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public static ApiResult<T> Ok(T data, HttpStatusCode code = HttpStatusCode.OK) => new() { Success = true, Data = data, StatusCode = code };
    public static ApiResult<T> Fail(string message, HttpStatusCode code = HttpStatusCode.BadRequest) => new() { Success = false, Error = message, StatusCode = code };
}