using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.Web.Models.Articles;
 public class CreateArticleViewModel
{
    [Display(Name = "Title")]
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Contenido")]
    [Required(ErrorMessage = "El contenido es obligatorio.")]
    public string Content { get; set; } = string.Empty;

    public DateTime PublishedDate { get; set; }
}
