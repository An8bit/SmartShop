using Microsoft.AspNetCore.Mvc;
using SmartShop.Core.Interfaces;

namespace SmartShop.Web.Controllers
{
    [Route("api/category")]
    public class CategoriesController : Controller
    {


        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("Id")]
        public IActionResult Index()
        {
            var categories = _categoryService.GetAllCategoriesAsync().Result;
            return View();
        }
    }
}
