namespace ArticleManagement.Web.Models.Articles;

public class CommentDto
{
    public Guid id { get; set; }            // asumiendo que el comentario tiene id
    public string text { get; set; } = null!;
    public string userName { get; set; } = null!;
    public DateTime createdAt { get; set; }
}
