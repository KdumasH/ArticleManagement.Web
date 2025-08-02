using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.Web.Models.Articles;

public class EditArticleViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    public string Title { get; set; } = string.Empty;

    public DateTime PublishedDate { get; set; }

    [Required(ErrorMessage = "El contenido es obligatorio")]
    public string Content { get; set; } = string.Empty;
}
