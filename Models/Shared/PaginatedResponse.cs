using System.Text.Json.Serialization;

namespace ArticleManagement.Web.Models.Shared;

public class PaginatedResponse<T>
{
    [JsonPropertyName("items")]
    public IReadOnlyList<T> Items { get; init; } = new List<T>();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; init; }

    public PaginatedResponse() { }

    public PaginatedResponse(PagedList<T> pagedList)
    {
        Items = pagedList.Items;
        Pagination = new PaginationMetadata(
            pagedList.TotalCount,
            pagedList.PageIndex,
            pagedList.PageSize,
            pagedList.TotalPages,
            pagedList.HasPreviousPage,
            pagedList.HasNextPage
        );
    }
    //    public IReadOnlyList<T> Items { get; init; } = new List<T>();
    //    public PaginationMetadata Pagination { get; init; }

    //    public PaginatedResponse(PagedList<T> pagedList)
    //    {
    //        Items = pagedList.Items;
    //        Pagination = new PaginationMetadata(
    //            pagedList.TotalCount,
    //            pagedList.PageIndex,
    //            pagedList.PageSize,
    //            pagedList.TotalPages,
    //            pagedList.HasPreviousPage,
    //            pagedList.HasNextPage
    //        );
    //    }

    //    public PaginatedResponse() { }
}