namespace FileGateway.Application;

public record class ApiResult<T>
{
    public bool Success { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; }
}
