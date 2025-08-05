using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SmartShop.Web.Pages.Products
{
    public class ProductEditModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductEditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductEditDto Product { get; set; } = new();

        [BindProperty]
        public List<VariantEditDto> Variants { get; set; } = new();

        public List<CategoryDto> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm.";
                    return RedirectToPage("./ListProducts");
                }

                Product = new ProductEditDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    ImageUrl = product.ImageUrl
                };

                Variants = product.Variants?.Select(v => new VariantEditDto
                {
                    Color = v.Color,
                    Size = v.Size,
                    StockQuantity = v.StockQuantity
                }).ToList() ?? new List<VariantEditDto>();

                await LoadCategoriesAsync();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải thông tin sản phẩm: {ex.Message}";
                return RedirectToPage("./ListProducts");
            }
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                Categories = await _productService.GetCategoriesAsync();
            }
            catch
            {
                Categories = new List<CategoryDto>
                {
                    new CategoryDto { CategoryId = 1, CategoryName = "Nam" },
                    new CategoryDto { CategoryId = 2, CategoryName = "Nữ" },
                    new CategoryDto { CategoryId = 3, CategoryName = "Unisex" },
                    new CategoryDto { CategoryId = 4, CategoryName = "Phụ kiện" }
                };
                TempData["ErrorMessage"] = "Không thể tải danh mục từ API, sử dụng danh mục mặc định.";
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            try
            {
                // Lọc bỏ các variant trống
                Variants = Variants?.Where(v => !string.IsNullOrWhiteSpace(v.Color) && !string.IsNullOrWhiteSpace(v.Size)).ToList() ?? new List<VariantEditDto>();

                // Validation
                if (string.IsNullOrWhiteSpace(Product.Name))
                    ModelState.AddModelError(nameof(Product.Name), "Tên sản phẩm không được để trống");

                if (Product.Price <= 0)
                    ModelState.AddModelError(nameof(Product.Price), "Giá sản phẩm phải lớn hơn 0");

                if (Product.CategoryId <= 0)
                    ModelState.AddModelError(nameof(Product.CategoryId), "Vui lòng chọn danh mục");

                if (!Variants.Any())
                    ModelState.AddModelError("Variants", "Vui lòng thêm ít nhất một biến thể");

                foreach (var variant in Variants)
                {
                    if (variant.StockQuantity < 0)
                        ModelState.AddModelError("Variants", "Số lượng tồn kho không được âm");
                }

                if (!ModelState.IsValid)
                {
                    await LoadCategoriesAsync();
                    TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin đã nhập";
                    return Page();
                }

                var productDto = new ProductDto
                {
                    ProductId = Product.ProductId,
                    Name = Product.Name.Trim(),
                    Description = Product.Description?.Trim() ?? "",
                    Price = Product.Price,
                    CategoryId = Product.CategoryId,
                    ImageUrl = Product.ImageUrl?.Trim() ?? "",
                    Variants = Variants.Select(v => new VariantDto
                    {
                        Color = v.Color.Trim(),
                        Size = v.Size.Trim(),
                        StockQuantity = v.StockQuantity
                    }).ToList()
                };

                var result = await _productService.UpdateProductAsync(productDto);

                if (result != null)
                {
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
                    return RedirectToPage("./ListProducts");
                }

                TempData["ErrorMessage"] = "Cập nhật sản phẩm thất bại.";
                await LoadCategoriesAsync();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật sản phẩm: {ex.Message}";
                await LoadCategoriesAsync();
                return Page();
            }
        }
    }

    public class ProductEditDto
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được quá 200 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được quá 1000 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục hợp lệ")]
        public int CategoryId { get; set; }

        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class VariantEditDto
    {
        [Required(ErrorMessage = "Màu sắc không được để trống")]
        [StringLength(50, ErrorMessage = "Màu sắc không được quá 50 ký tự")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kích thước không được để trống")]
        [StringLength(50, ErrorMessage = "Kích thước không được quá 50 ký tự")]
        public string Size { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm")]
        public int StockQuantity { get; set; }
    }
}
