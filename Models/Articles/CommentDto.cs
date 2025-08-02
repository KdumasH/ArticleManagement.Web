namespace ArticleManagement.Web.Models.Articles;

public class CommentDto
{
    public Guid id { get; set; }            // asumiendo que el comentario tiene id
    public string content { get; set; } = null!;
    public string author { get; set; } = null!;
    public DateTime createdAt { get; set; }
}
