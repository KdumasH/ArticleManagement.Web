namespace ArticleManagement.Web.Models.Articles;

public class ArticleViewModel
{
  public Guid Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;
  public string Author { get; set; } = string.Empty;
  public DateTime PublishedDate { get; set; }
}