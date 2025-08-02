using Newtonsoft.Json;

namespace ArticleManagement.Web.Models.Articles;

public class CreateArticleCommandResponse
{
    [JsonProperty("id")] // si usas Newtonsoft.Json
    public Guid Id { get; set; }
}
