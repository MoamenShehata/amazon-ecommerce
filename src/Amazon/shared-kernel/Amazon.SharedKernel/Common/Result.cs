namespace Amazon.SharedKernel.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public Exception? Exception { get; set; }
    protected Result(bool isSuccess, string error, Exception exception = null)
    {
        if (isSuccess && error != string.Empty)
            throw new InvalidOperationException();

        if (!isSuccess && error == string.Empty)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
        Exception = exception;
    }

    public static Result Success() => new Result(true, string.Empty);
    public static Result Failure(string error, Exception exception = null) => new Result(false, error, exception);
}

public class Result<T> : Result
{
    public T Value { get; }

    protected Result(T value, bool isSuccess, string error, Exception exception = null)
        : base(isSuccess, error, exception)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(value, true, string.Empty);
    public new static Result<T> Failure(string error, Exception exception = null) => new Result<T>(default!, false, error, exception);

    public static implicit operator T(Result<T> source) => source.Value;
}
