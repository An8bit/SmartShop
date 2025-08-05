using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;

namespace SmartShop.Web.Pages.Products
{
    public class ProductsModel(IProductService productService) : PageModel
    {
        private readonly IProductService _productService = productService;

        public IList<ProductDto> Products { get; set; } = new List<ProductDto>();
        public async Task OnGetAsync()
        {
            try
            {
                var result = await _productService.GetAllProductsAsync();
                Products = result?.ToList() ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải danh sách sản phẩm: {ex.Message}";
                Products = new List<ProductDto>(); // Đảm bảo Products không null
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                    result ? "Xóa sản phẩm thành công" : "Không thể xóa sản phẩm. Vui lòng thử lại.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa sản phẩm: {ex.Message}";
            }

            return RedirectToPage();
        }

    }
}