using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SmartShop.Web.Pages.Products
{
    public class ProductCreateModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductCreateModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductCreateDto Product { get; set; } = new ProductCreateDto();

        [BindProperty]
        public List<VariantCreateDto> Variants { get; set; } = new List<VariantCreateDto>();

        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        public async Task OnGetAsync()
        {
            // Initialize with one empty variant
            Variants.Add(new VariantCreateDto());
            
            // Load categories from API
            await LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                Categories = await _productService.GetCategoriesAsync();
            }
            catch (Exception)
            {
                // If API fails, use default categories
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Remove empty variants
                Variants = Variants.Where(v => !string.IsNullOrWhiteSpace(v.Color) && !string.IsNullOrWhiteSpace(v.Size)).ToList();

                // Validate required fields manually
                if (string.IsNullOrWhiteSpace(Product.Name))
                {
                    ModelState.AddModelError(nameof(Product.Name), "Tên sản phẩm không được để trống");
                }

                if (Product.Price <= 0)
                {
                    ModelState.AddModelError(nameof(Product.Price), "Giá sản phẩm phải lớn hơn 0");
                }

                if (Product.CategoryId <= 0)
                {
                    ModelState.AddModelError(nameof(Product.CategoryId), "Vui lòng chọn danh mục");
                }

                if (!Variants.Any())
                {
                    ModelState.AddModelError("Variants", "Vui lòng thêm ít nhất một biến thể");
                }

                // Validate variants
                foreach (var variant in Variants)
                {
                    if (variant.StockQuantity < 0)
                    {
                        ModelState.AddModelError("Variants", "Số lượng tồn kho không được âm");
                    }
                }

                if (!ModelState.IsValid)
                {
                    await LoadCategoriesAsync();
                    TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin đã nhập";
                    return Page();
                }

                // Create ProductDto from form data
                var productDto = new ProductDto
                {
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

                var result = await _productService.CreateProductAsync(productDto);
                
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToPage("./ListProducts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi thêm sản phẩm: {ex.Message}";
                await LoadCategoriesAsync();
                return Page();
            }
        }
    }

    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được quá 200 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được quá 1000 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục hợp lệ")]
        public int CategoryId { get; set; }

        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class VariantCreateDto
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
