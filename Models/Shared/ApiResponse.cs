namespace ArticleManagement.Web.Models.Shared;
public class ApiResponse<T>
{
    public T Value { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public bool IsFailure { get; set; }
    public ApiError Error { get; set; } = new();
}