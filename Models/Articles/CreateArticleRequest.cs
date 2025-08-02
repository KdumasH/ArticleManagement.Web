namespace ArticleManagement.Web.Models.Articles;

public class CreateArticleRequest
{
    public Guid IdUser { get; set; }


    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
}
