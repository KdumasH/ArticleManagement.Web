using System.Text.Json.Serialization;

namespace ArticleManagement.Web.Models.Shared;

public class Result<T>
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; private set; }

    [JsonPropertyName("value")]
    public T? Value { get; private set; }

    [JsonPropertyName("errors")]
    public Error[]? Errors { get; private set; }

    public Result() { }

    private Result(bool isSuccess, T? value, Error[]? errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T value)
        => new(true, value, null);

    public static Result<T> Failure(params Error[] errors)
        => new(false, default, errors);
    //public bool IsSuccess { get; private set; }
    //public T? Value { get; private set; }
    //public Error[]? Errors { get; private set; }

    //private Result(bool isSuccess, T? value, Error[]? errors)
    //{
    //    IsSuccess = isSuccess;
    //    Value = value;
    //    Errors = errors;
    //}
    //public Result() { }

    //public static Result<T> Success(T value)
    //{
    //    return new Result<T>(true, value, null);
    //}

    //public static Result<T> Failure(params Error[] errors)
    //{
    //    return new Result<T>(false, default, errors);
    //}
}