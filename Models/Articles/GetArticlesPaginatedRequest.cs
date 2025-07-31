namespace ArticleManagement.Web.Models.Articles;

public class GetArticlesPaginatedRequest
{
  public string? Title { get; set; }
  public string? Author { get; set; }
  public DateTime? PublishedAfter { get; set; }
  public DateTime? PublishedBefore { get; set; }
  public string? OrderBy { get; set; } = "publishedDate";
  public string? SortDirection { get; set; } = "desc";
  public int PageIndex { get; set; } = 1;
  public int PageSize { get; set; } = 10;
}