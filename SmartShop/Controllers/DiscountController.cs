using Microsoft.AspNetCore.Mvc;

namespace SmartShop.Web.Controllers
{
    public class DiscountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
