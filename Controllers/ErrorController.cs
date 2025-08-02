using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ApiNoDisponible()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
