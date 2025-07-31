namespace ArticleManagement.Web.Models.Articles;

public class ArticleDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
    public string Content { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public List<CommentDto> Comments { get; set; } = new();
}
