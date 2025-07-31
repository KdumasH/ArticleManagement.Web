namespace ArticleManagement.Web.Models.Articles;

public class ArticleDetailViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public List<CommentViewModel> Comments { get; set; } = new();
}
