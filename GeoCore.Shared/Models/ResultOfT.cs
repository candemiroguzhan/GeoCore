namespace GeoCore.Shared.Models;

public sealed class Result<T> : Result
{
    private Result(bool succeeded, T? value, string? error) : base(succeeded, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(true, value, null);

    public new static Result<T> Failure(string error) => new(false, default, error);
}
