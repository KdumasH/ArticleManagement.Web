namespace ArticleManagement.Web.Models.Auth;

public class UserInfo
{
  public Guid Id { get; set; }
  public string Username { get; set; } = default!;
  public string Email { get; set; } = default!;
  public List<string> Roles { get; set; } = [];
}