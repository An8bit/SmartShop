using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;

namespace SmartShop.Web.Pages.Discounts
{
    public class CreateModel : PageModel
    {
        private readonly IDiscountService _discountService;
        private readonly IProductService _productService;

        public CreateModel(IDiscountService discountService, IProductService productService)
        {
            _discountService = discountService;
            _productService = productService;
        }

        [BindProperty]
        public DiscountCreateDto Discount { get; set; } = new DiscountCreateDto();

        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();

        public async Task OnGetAsync()
        {
            await LoadProductsAsync();
            // Set default values
            Discount.StartDate = DateTime.Today;
            Discount.EndDate = DateTime.Today.AddDays(7);
            
        }

        private async Task LoadProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            Products = products
                .Select(p => new SelectListItem
                {
                    Value = p.ProductId.ToString(),
                    Text = $"{p.Name} (ID: {p.ProductId})"
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadProductsAsync();
                return Page();
            }

            try
            {
                // Additional validation
                if (Discount.EndDate <= Discount.StartDate)
                {
                    ModelState.AddModelError("Discount.EndDate", "Ngày kết thúc phải sau ngày bắt đầu");
                    await LoadProductsAsync();
                    return Page();
                }

                await _discountService.CreateDiscountAsync(Discount);
                TempData["SuccessMessage"] = "Tạo chương trình giảm giá thành công!";
                return RedirectToPage("./ListDiscounts");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tạo chương trình giảm giá: {ex.Message}";
                await LoadProductsAsync();
                return Page();
            }
        }
    }
}