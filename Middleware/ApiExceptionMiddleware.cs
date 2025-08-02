using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace ArticleManagement.Web.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger, ITempDataDictionaryFactory tempDataFactory)
        {
            _next = next;
            _logger = logger;
            _tempDataFactory = tempDataFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con la API.");
                await ManejarError(context, "No se pudo conectar con el servicio. Intente más tarde.");
            }
            catch (SocketException ex)
            {
                _logger.LogError(ex, "La API no está alcanzable.");
                await ManejarError(context, "El servicio no está disponible en este momento.");
            }
        }

        private async Task ManejarError(HttpContext context, string mensaje)
        {
            bool esAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (esAjax)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 503; 
                var json = JsonSerializer.Serialize(new
                {
                    success = false,
                    toastMessage = mensaje,
                    toastType = "danger"
                });
                await context.Response.WriteAsync(json);
            }
            else
            {
                var tempData = _tempDataFactory.GetTempData(context);
                tempData["ToastMessage"] = mensaje;
                tempData["ToastType"] = "danger";
                tempData.Save();


                var referer = context.Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer))
                    context.Response.Redirect(referer);
                else
                    context.Response.Redirect("/");
            }
        }
    }
}
