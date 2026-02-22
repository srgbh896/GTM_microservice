using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto.Base;

/// <summary>
/// Base result type (non-generic).
/// Provides success/failure state and error information.
/// </summary>
public abstract class Result : IUseCaseOutput
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Error message describing why the operation failed.
    /// Empty when the operation succeeds.
    /// </summary>
    public string Error { get; }

    /// <summary>
    /// Result
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="error"></param>
    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error ?? string.Empty;
    }

    /// <summary>
    /// Creates a successful result containing a value.
    /// </summary>
    public static Result<T> Success<T>(T value)
        => new Result<T>(value);

    /// <summary>
    /// Creates a failed result with an error message.
    /// </summary>
    public static Result<T> Failure<T>(string error)
        => new Result<T>(error);
}

/// <summary>
/// Represents the result of an operation that returns a value.
/// </summary>
/// <typeparam name="T">Type of the value returned when successful.</typeparam>
public sealed class Result<T> : Result
{
    /// <summary>
    /// The value returned by the operation when successful.
    /// Will be default when the operation fails.
    /// </summary>
    public T Value { get; }

    internal Result(T value)
        : base(true, string.Empty)
    {
        Value = value;
    }

    internal Result(string error)
        : base(false, error)
    {
        Value = default;
    }
}
