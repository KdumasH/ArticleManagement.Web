namespace ArticleManagement.Web.Models.Articles;

public class ArticleDetailDto
{
    public Guid id { get; set; }
    public string title { get; set; } = null!;
    public DateTime publishedDate { get; set; }
    public string content { get; set; } = null!;
    public string userName { get; set; } = null!;
    public List<CommentDto> comments { get; set; } = new();
}
