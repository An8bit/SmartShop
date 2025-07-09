using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;

namespace SmartShop.Web.Pages.Products
{
    public class ProductsModel(IProductService productService) : PageModel
    {
        private readonly IProductService _productService = productService;

        public required IList<ProductDto> Products { get; set; }
        public async Task OnGetAsync()
        {
            try
            {
                var result = await _productService.GetAllProductsAsync();
                Products = result.ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"L?i khi t?i danh s�ch s?n ph?m: {ex.Message}";
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "X�a s?n ph?m th�nh c�ng";
                }
                else
                {
                    TempData["ErrorMessage"] = "Kh�ng th? x�a s?n ph?m";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"L?i khi x�a s?n ph?m: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}