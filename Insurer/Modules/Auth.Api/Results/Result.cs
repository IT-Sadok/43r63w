namespace Auth.Api.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = error;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, null);
    public static Result<T> Failure(string errorMessage) => new Result<T>(false, default(T), errorMessage);
}

