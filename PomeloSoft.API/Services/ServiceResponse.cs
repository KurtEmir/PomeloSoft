namespace PomeloSoft.API.Services;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string? Message { get; set; }

    public static ServiceResponse<T> CreateSuccess(T data)
    {
        return new ServiceResponse<T> { Data = data };
    }

    public static ServiceResponse<T> CreateFailure(string message)
    {
        return new ServiceResponse<T> { Success = false, Message = message };
    }
} 