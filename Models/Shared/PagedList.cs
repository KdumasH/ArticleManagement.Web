namespace ArticleManagement.Web.Models.Shared;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageIndex > 1; //pagination starts at 1
    public bool HasNextPage => PageIndex < TotalPages;

    public PagedList(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items.ToList();
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public static PagedList<T> Create(IEnumerable<T> source, int totalCount, int pageIndex, int pageSize)
        => new(source, totalCount, pageIndex, pageSize);
}