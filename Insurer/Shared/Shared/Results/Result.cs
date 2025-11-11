using Shared.Results;

namespace Shared.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string? ErrorMessage { get; }
    public Dictionary<string,string>? Errors { get; }
    private Result(bool isSuccess, T? value, string? error, Dictionary<string,string>? errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = error;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, null, null);
    public static Result<T> Failure(string errorMessage, Dictionary<string,string>? errors = null) => new Result<T>(false, default(T), errorMessage, errors);
}

