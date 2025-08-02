namespace ArticleManagement.Web.Models.Articles;

public class UpdateArticleRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string Content { get; set; } = string.Empty;
}
