using Newtonsoft.Json;

namespace ArticleManagement.Web.Models.Articles;

public class CreateArticleCommandResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
}
