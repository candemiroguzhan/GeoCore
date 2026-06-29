namespace GeoCore.Shared.Models;

public sealed record ApiResponse<T>(bool Succeeded, T? Data, string? Message = null, IReadOnlyCollection<string>? Errors = null)
{
    public static ApiResponse<T> Success(T data, string? message = null) => new(true, data, message);

    public static ApiResponse<T> Failure(params string[] errors) => new(false, default, null, errors);
}
