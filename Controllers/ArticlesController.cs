using ArticleManagement.Web.Models.Articles;
using ArticleManagement.Web.Models.Shared;
using ArticleManagement.Web.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetArticlesPaginatedRequest request)
        {

            var result = await _articleService.GetPaginatedAsync(request);
            if (result == null || !result.IsSuccess || result.Value == null)
            {
                return View(new PaginatedResponse<ArticleViewModel>(new PagedList<ArticleViewModel>([], 0, request.PageIndex, request.PageSize)));
            }

            var paginatedDto = result.Value;

            var viewModels = paginatedDto.Items.Select(dto => new ArticleViewModel
            {
                Id = dto.id,
                Title = dto.title,
                Content = dto.content,
                PublishedDate = dto.publishedDate,
                Author = dto.userName
            }).OrderByDescending(a => a.PublishedDate).ToList();

            var paginatedViewModel = new PaginatedResponse<ArticleViewModel>(
                new PagedList<ArticleViewModel>(
                    viewModels,
                    paginatedDto.Pagination.TotalCount,
                    paginatedDto.Pagination.PageIndex,
                    paginatedDto.Pagination.PageSize
                ));

            return View(paginatedViewModel);
            //var result = await _articleService.GetPaginatedAsync(request);
            //if (result == null || !result.IsSuccess || result.Value == null)
            //{
            //    return View(new PaginatedResponse<ArticleViewModel>(new PagedList<ArticleViewModel>([], 0, request.PageIndex, request.PageSize)));
            //}

            //// Mapear cada ArticleDto a ArticleViewModel
            //var mappedItems = _mapper.Map<List<ArticleViewModel>>(result.Value.Items);

            //// Crear el nuevo pagedList manualmente con la paginación del original
            //var mappedPagedList = new PagedList<ArticleViewModel>(
            //    mappedItems,
            //    result.Value.Pagination.TotalCount,
            //    result.Value.Pagination.PageIndex,
            //    result.Value.Pagination.PageSize
            //);

            //// Crear el PaginatedResponse nuevo
            //var mappedResponse = new PaginatedResponse<ArticleViewModel>(mappedPagedList);

            //return View(mappedResponse);

            //////if (result == null || !result.IsSuccess || result.Value == null)
            //////{
            //////  // Puedes mostrar un mensaje de error o una vista vacía
            //////  return View(new PaginatedResponse<ArticleDto>(new PagedList<ArticleDto>([], 0, request.PageIndex, request.PageSize)));
            //////}

            ////return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _articleService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Value == null)
            {
                // Puedes mostrar una vista de error o redirigir
                return View("Error", result.Errors);
            }

            // Mapeo manual usando AutoMapper desde ArticleDetailDto a ArticleDetailViewModel
            var viewModel = _mapper.Map<ArticleDetailViewModel>(result.Value);

            return View(viewModel);
        }

    }
}
