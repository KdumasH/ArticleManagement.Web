namespace ArticleManagement.Web.Models.Auth;

public class LoginResult
{
  public Guid UserId { get; set; }
  public string Username { get; set; } = default!;
  public string Email { get; set; } = default!;
  public List<string> Roles { get; set; } = [];
  public string Token { get; set; } = default!;
  public DateTime Expiration { get; set; }
}