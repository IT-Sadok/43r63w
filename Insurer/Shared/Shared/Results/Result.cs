using Shared.Results;

namespace Shared.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string? ErrorMessage { get; }
    public IEnumerable<string>? Errors { get; }

    private Result(bool isSuccess, T? value, string? error, IEnumerable<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = error;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, null, null);
    public static Result<T> Failure(string errorMessage, IEnumerable<string>? errors = null) => new Result<T>(false, default(T), errorMessage, errors);
}

