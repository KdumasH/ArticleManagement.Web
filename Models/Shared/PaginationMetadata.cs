namespace ArticleManagement.Web.Models.Shared;
public class PaginationMetadata
{
  public int TotalCount { get; set; }
  public int PageIndex { get; set; }
  public int PageSize { get; set; }
  public int TotalPages { get; set; }
  public bool HasPreviousPage { get; set; }
  public bool HasNextPage { get; set; }

  public PaginationMetadata() { }

  public PaginationMetadata(int totalCount, int pageIndex, int pageSize, int totalPages, bool hasPrevious, bool hasNext)
  {
    TotalCount = totalCount;
    PageIndex = pageIndex;
    PageSize = pageSize;
    TotalPages = totalPages;
    HasPreviousPage = hasPrevious;
    HasNextPage = hasNext;
  }
}