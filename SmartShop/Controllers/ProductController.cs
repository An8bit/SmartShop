using Microsoft.AspNetCore.Mvc;
using SmartShop.Core.Interfaces;

namespace SmartShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

       
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View("ListProducts", products);
        }
    }
}
