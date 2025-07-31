namespace ArticleManagement.Web.Models.Articles;

public class ArticleDto
{
  public Guid id { get; set; }
  public string title { get; set; } = null!;
  public string content { get; set; } = null!;
  public Guid userId { get; set; }
  public string userName { get; set; } = null!;
  public DateTime publishedDate { get; set; }
}