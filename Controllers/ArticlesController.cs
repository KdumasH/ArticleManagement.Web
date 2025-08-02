using ArticleManagement.Web.Models.Articles;
using ArticleManagement.Web.Models.Shared;
using ArticleManagement.Web.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.Web.Controllers;

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
    }

    [HttpGet]
    public async Task<IActionResult> ArticleList([FromQuery] GetArticlesPaginatedRequest request)
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
    }

    //var viewModels = paginatedDto.Items.Select(dto => new ArticleViewModel
    //{
    //    Id = dto.id,
    //    Title = dto.title,
    //    Content = dto.content,
    //    PublishedDate = dto.publishedDate,
    //    Author = dto.userName
    //}).OrderByDescending(a => a.PublishedDate).ToList();

    //var paginatedViewModel = new PaginatedResponse<ArticleViewModel>(
    //    new PagedList<ArticleViewModel>(
    //        viewModels,
    //        paginatedDto.Pagination.TotalCount,
    //        paginatedDto.Pagination.PageIndex,
    //        paginatedDto.Pagination.PageSize
    //    ));

    //        return View(paginatedViewModel);
    //        //return View(listaArticulo);
    //        //var listaArticulo = new List<ArticleViewModel>() 
    //        //{ 
    //        //    new ArticleViewModel
    //        //    {
    //        //        Id = Guid.Parse("dc245142-dc55-4e8a-b4f9-ef6744c66195"),
    //        //        Title = "Articulo 1",
    //        //        Content = "Contenido del articulo 1",
    //        //        PublishedDate = DateTime.Now,
    //        //        Author = "Autor 1"
    //        //    },
    //        //    new ArticleViewModel
    //        //    {
    //        //        Id = Guid.Parse("dc245142-dc55-4e8a-b4f9-ef6744c66195"),
    //        //        Title = "Articulo 2",
    //        //        Content = "Contenido del articulo 2",
    //        //        PublishedDate = DateTime.Now,
    //        //        Author = "Autor 2"
    //        //    },
    //        //    new ArticleViewModel
    //        //    {
    //        //        Id = Guid.Parse("dc245142-dc55-4e8a-b4f9-ef6744c66195"),
    //        //        Title = "Articulo 3",
    //        //        Content = "Contenido del articulo 3",
    //        //        PublishedDate = DateTime.Now,
    //        //        Author = "Autor 3"
    //        //    }
    //        //};
    //    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _articleService.GetByIdAsync(id);

        if (!result.IsSuccess || result.Value == null)
        {
            // Puedes mostrar una vista de error o redirigir
            return View("Error", result.Errors);
        }


        // Enviar a la vista
        ViewBag.CurrentUser = HttpContext.Session.GetString("UserId");

        var viewModel = _mapper.Map<ArticleDetailViewModel>(result.Value);
        //ViewData.Add("UserID")

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateArticleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            ModelState.AddModelError(string.Empty, "No se pudo obtener el usuario autenticado.");
            return View(model);
        }

        var request = _mapper.Map<CreateArticleRequest>(model);
        request.IdUser = userId;
        request.PublishedDate = DateTime.Now;

        var result = await _articleService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }
            return View(model);
        }

        // Registro exitoso → redirigir a la lista
        TempData["ToastMessage"] = "El registro de artículo fue exitoso";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _articleService.GetByIdAsync(id);
        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound();
        }

        var model = _mapper.Map<EditArticleViewModel>(result.Value);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditArticleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.PublishedDate == default)
        {
            model.PublishedDate = DateTime.Now;
        }

        var request = _mapper.Map<UpdateArticleRequest>(model);
        var result = await _articleService.UpdateAsync(request);

        if (!result.IsSuccess)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }
            return View(model);
        }

        TempData["ToastMessage"] = "El artículo fue editado exitosamente";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var currentUserId = HttpContext.Session.GetString("UserId");
        var isAdmin = HttpContext.Session.GetString("Roles")?.Contains("Admin") ?? false; ;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Json(new { success = false, message = "No está autenticado." });
        }

        var result = await _articleService.GetByIdAsync(id);

        if (!result.IsSuccess || result.Value == null)
        {
            var errorMsg = result.Errors != null && result.Errors.Length > 0
                ? string.Join(", ", result.Errors.Select(e => e.Message))
                : "Artículo no encontrado.";

            return Json(new { success = false, message = errorMsg });
        }

        var article = result.Value;

        if (article.CreatedByUserId != currentUserId && !isAdmin)
        {
            return Json(new { success = false, message = "No tiene permisos para eliminar este artículo." });
        }

        var deleteResult = await _articleService.DeleteAsync(id);

        if (!deleteResult.IsSuccess)
        {
            var deleteErrorMsg = deleteResult.Errors != null && deleteResult.Errors.Length > 0
                ? string.Join(", ", deleteResult.Errors.Select(e => e.Message))
                : "Error al eliminar el artículo.";

            return Json(new { success = false, message = deleteErrorMsg });
        }

        return Json(new { success = true, message = "Artículo eliminado correctamente." });
        //var result = await _articleService.DeleteAsync(id);

        //if (!result.IsSuccess)
        //{
        //    return Json(new { success = false, message = string.Join(" ", result.Errors.Select(e => e.Message)) });
        //}

        //return Json(new { success = true, message = "Artículo eliminado correctamente." });
    }
}
