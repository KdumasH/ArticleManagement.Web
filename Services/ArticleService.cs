using ArticleManagement.Web.Models.Articles;
using ArticleManagement.Web.Models.Shared;
using ArticleManagement.Web.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ArticleManagement.Web.Services;

public class ArticleService : IArticleService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public ArticleService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<PaginatedResponse<ArticleDto>>> GetPaginatedAsync(GetArticlesPaginatedRequest request)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        // Construcción de los query params
        var queryParams = new Dictionary<string, string?>
    {
        { "title", request.Title },
        { "author", request.Author },
        { "publishedAfter", request.PublishedAfter?.ToString("o") },
        { "publishedBefore", request.PublishedBefore?.ToString("o") },
        { "orderBy", request.OrderBy },
        { "sortDirection", request.SortDirection },
        { "pageIndex", request.PageIndex.ToString() },
        { "pageSize", request.PageSize.ToString() }
    };

        var query = string.Join("&", queryParams
            .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
            .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value!)}"));

        var response = await client.GetAsync($"/api/articles/paginated?{query}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return Result<PaginatedResponse<ArticleDto>>.Failure(
                new Error("API_ERROR", $"Error {response.StatusCode}: {errorContent}")
            );
        }

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);

        var apiResult = JsonConvert.DeserializeObject<ApiResponse<PaginatedResponse<ArticleDto>>>(json);

        if (apiResult == null)
        {
            return Result<PaginatedResponse<ArticleDto>>.Failure(
                new Error("DESERIALIZATION_ERROR", "No se pudo deserializar la respuesta del servidor.")
            );
        }

        if (!apiResult.IsSuccess)
        {
            return Result<PaginatedResponse<ArticleDto>>.Failure(
                new Error("API_FAILURE", "La operación no fue exitosa.")
            );
        }

        if (apiResult.Value == null || apiResult.Value.Items == null || apiResult.Value.Pagination == null)
        {
            return Result<PaginatedResponse<ArticleDto>>.Failure(
                new Error("NULL_DATA", "La respuesta de la API no contiene datos válidos.")
            );
        }

        return Result<PaginatedResponse<ArticleDto>>.Success(apiResult.Value);
    }

    public async Task<Result<ArticleDetailDto>> GetByIdAsync(Guid id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        var response = await client.GetAsync($"/api/articles/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return Result<ArticleDetailDto>.Failure(
                new Error("API_ERROR", $"Error {response.StatusCode}: {errorContent}")
            );
        }

        var json = await response.Content.ReadAsStringAsync();

        var article = JsonConvert.DeserializeObject<ArticleDetailDto>(json);

        if (article == null)
        {
            return Result<ArticleDetailDto>.Failure(
                new Error("DESERIALIZATION_ERROR", "No se pudo deserializar el artículo.")
            );
        }

        return Result<ArticleDetailDto>.Success(article);
    }

    public async Task<Result<Guid>> CreateAsync(CreateArticleRequest request)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        if (string.IsNullOrEmpty(token))
        {
            return Result<Guid>.Failure(
                new Error("UNAUTHORIZED", "No se ha proporcionado un token de autenticación.")
            );
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("api/articles", request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return Result<Guid>.Failure(
                new Error("API_ERROR", $"Error {response.StatusCode}: {content}")
            );
        }

        var createResult = JsonConvert.DeserializeObject<CreateArticleCommandResponse>(content);

        if (createResult == null || createResult.Id == Guid.Empty)
        {
            return Result<Guid>.Failure(
                new Error("API_FAILURE", $"No se pudo obtener el ID: {content}")
            );
        }

        return Result<Guid>.Success(createResult.Id);
    }

    public async Task<Result<bool>> UpdateAsync(UpdateArticleRequest request)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        if (string.IsNullOrEmpty(token))
        {
            return Result<bool>.Failure(
                new Error("UNAUTHORIZED", "No se ha proporcionado un token de autenticación.")
            );
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PutAsJsonAsync($"api/articles/{request.Id}", request);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Result<bool>.Failure(
                new Error("API_ERROR", $"Error {response.StatusCode}: {content}")
            );
        }

        // API devolvió 204 NoContent, lo consideramos éxito
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        if (string.IsNullOrEmpty(token))
        {
            return Result<bool>.Failure(
                new Error("UNAUTHORIZED", "No se ha proporcionado un token de autenticación.")
            );
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync($"api/articles/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Result<bool>.Failure(
                new Error("API_ERROR", $"Error {response.StatusCode}: {content}")
            );
        }

        return Result<bool>.Success(true);
    }
}