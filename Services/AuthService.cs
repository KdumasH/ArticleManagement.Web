using System.Net.Http.Headers;
using System.Security.Claims;
using ArticleManagement.Web.Models.Auth;
using ArticleManagement.Web.Services.Interfaces;

namespace ArticleManagement.Web.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LoginResult?> LoginAsync(string username, string password)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("/api/auth/login", new { username, password });

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResult>();
        
        if (result.UserId != Guid.Empty)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("UserId", result.UserId.ToString());
        }

        // Guardar token en sesión
        _httpContextAccessor.HttpContext?.Session.SetString("AuthToken", result!.Token);

        return result;
    }

    public async Task<bool> RegisterAsync(string username, string email, string password)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("/api/auth/register", new { username, email, password });

        if (!response.IsSuccessStatusCode)
            return false;

        //var result = await response.Content.ReadFromJsonAsync<RegisterResult>();
        return true;
    }

    public async Task<UserInfo?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

        if (string.IsNullOrEmpty(token))
            return null;

        var client = _httpClientFactory.CreateClient("ArticlesApi");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/auth/me");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserInfo>();
    }
}