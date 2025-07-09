using Microsoft.AspNetCore.Mvc;

namespace SmartShop.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
