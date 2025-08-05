using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;

namespace SmartShop.Web.Pages.Products
{
    public class ProductDetailModel(IProductService productService) : PageModel

    {
        private readonly IProductService _productService = productService;

        [BindProperty]
        public ProductDto? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
        Product = await _productService.GetProductByIdAsync(id); // <- Phải gán vào biến Product

        if (Product == null)
        {
        TempData["ErrorMessage"] = "Không tìm thấy sản phẩm.";
        return RedirectToPage("/Products/ListProducts");
        }

        return Page();
        }

    }
}
