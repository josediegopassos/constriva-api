namespace Constriva.Application.Common.Implementations;

public class Result
{
    public bool Success { get; protected set; }
    public string? Error { get; protected set; }

    public static Result Ok() => new() { Success = true };
    public static Result Fail(string error) => new() { Success = false, Error = error };
}

public class Result<T> : Result
{
    public T? Data { get; private set; }

    public static Result<T> Ok(T data) => new() { Success = true, Data = data };
    public new static Result<T> Fail(string error) => new() { Success = false, Error = error };
}
