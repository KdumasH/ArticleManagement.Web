using ArticleManagement.Web.Models.Articles;
using ArticleManagement.Web.Models.Shared;

namespace ArticleManagement.Web.Services.Interfaces;

public interface IArticleService
{
    Task<Result<PaginatedResponse<ArticleDto>>> GetPaginatedAsync(GetArticlesPaginatedRequest request);
    //Task<Result<PaginatedResponse<ArticleDto>>> GetPaginatedAsync(GetArticlesPaginatedRequest request);
    Task<Result<ArticleDetailDto>> GetByIdAsync(Guid id);

    Task<Result<Guid>> CreateAsync(CreateArticleRequest request);
    Task<Result<bool>> UpdateAsync(UpdateArticleRequest request);
    Task<Result<bool>> DeleteAsync(Guid id);

}