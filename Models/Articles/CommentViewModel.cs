namespace ArticleManagement.Web.Models.Articles
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
