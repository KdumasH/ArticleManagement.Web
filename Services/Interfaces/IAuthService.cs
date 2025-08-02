using System.Security.Claims;
using ArticleManagement.Web.Models.Auth;

namespace ArticleManagement.Web.Services.Interfaces;

public interface IAuthService
{
  Task<LoginResult?> LoginAsync(string username, string password);
  Task<bool> RegisterAsync(string username, string email, string password, bool isAdmin);
  Task<UserInfo?> GetCurrentUserAsync(ClaimsPrincipal user);
}
