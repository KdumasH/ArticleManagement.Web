using ArticleManagement.Web.Models.Articles;
using ArticleManagement.Web.Models.Shared;
using ArticleManagement.Web.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;

namespace ArticleManagement.Web.Services;

public class ArticleService : IArticleService
{
    private readonly IHttpClientFactory _httpClientFactory;


    public ArticleService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

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


    //public async Task<Result<PaginatedResponse<ArticleViewModel>>> GetPaginatedAsync(GetArticlesPaginatedRequest request)
    //{
    //    var client = _httpClientFactory.CreateClient("ApiClient");

    //    // Construcción de los query params
    //    var queryParams = new Dictionary<string, string?>
    //{
    //    { "title", request.Title },
    //    { "author", request.Author },
    //    { "publishedAfter", request.PublishedAfter?.ToString("o") },
    //    { "publishedBefore", request.PublishedBefore?.ToString("o") },
    //    { "orderBy", request.OrderBy },
    //    { "sortDirection", request.SortDirection },
    //    { "pageIndex", request.PageIndex.ToString() },
    //    { "pageSize", request.PageSize.ToString() }
    //};

    //    var query = string.Join("&", queryParams
    //        .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
    //        .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value!)}"));

    //    var response = await client.GetAsync($"/api/articles/paginated?{query}");

    //    if (!response.IsSuccessStatusCode)
    //    {
    //        var errorContent = await response.Content.ReadAsStringAsync();
    //        return Result<PaginatedResponse<ArticleViewModel>>.Failure(
    //            new Error("API_ERROR", $"Error {response.StatusCode}: {errorContent}")
    //        );
    //    }

    //    var json = await response.Content.ReadAsStringAsync();
    //    Console.WriteLine(json);

    //    var apiResult = JsonConvert.DeserializeObject<ApiResponse<PaginatedResponse<ArticleDto>>>(json);

    //    if (apiResult == null)
    //    {
    //        return Result<PaginatedResponse<ArticleViewModel>>.Failure(
    //            new Error("DESERIALIZATION_ERROR", "No se pudo deserializar la respuesta del servidor.")
    //        );
    //    }

    //    if (!apiResult.IsSuccess)
    //    {
    //        return Result<PaginatedResponse<ArticleViewModel>>.Failure(
    //            new Error("API_FAILURE", "La operación no fue exitosa.")
    //        );
    //    }

    //    if (apiResult.Value == null || apiResult.Value.Items == null || apiResult.Value.Pagination == null)
    //    {
    //        return Result<PaginatedResponse<ArticleViewModel>>.Failure(
    //            new Error("NULL_DATA", "La respuesta de la API no contiene datos válidos.")
    //        );
    //    }

    //    // Mapeo manual ArticleDto → ArticleViewModel
    //    var viewModels = apiResult.Value.Items.Select(dto => new ArticleViewModel
    //    {
    //        Id = dto.id,
    //        Title = dto.title,
    //        Content = dto.content,
    //        PublishedDate = dto.publishedDate,
    //        Author = dto.userName
    //    }).ToList();

    //    var paginated = new PaginatedResponse<ArticleViewModel>(
    //        new PagedList<ArticleViewModel>(
    //            viewModels,
    //            apiResult.Value.Pagination.TotalCount,
    //            apiResult.Value.Pagination.PageIndex,
    //            apiResult.Value.Pagination.PageSize
    //        ));

    //    return Result<PaginatedResponse<ArticleViewModel>>.Success(paginated);
    //}


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


}