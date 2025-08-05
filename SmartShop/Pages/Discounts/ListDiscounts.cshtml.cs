using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;

namespace SmartShop.Web.Pages.Discounts
{
    public class ListDiscountsModel : PageModel
    {
        private readonly IDiscountService _discountService;

        public ListDiscountsModel(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public IList<DiscountDto> Discounts { get; set; } = new List<DiscountDto>();
        public bool ShowActiveOnly { get; set; } = true;

        public async Task OnGetAsync(bool showActiveOnly = true)
        {
            ShowActiveOnly = showActiveOnly;

            var discounts = await _discountService.GetAllDiscountsAsync();

            if (ShowActiveOnly)
            {
                var now = DateTime.Now;
                Discounts = discounts
                    .Where(d => d.IsActive && d.StartDate <= now && d.EndDate >= now)
                    .ToList();
            }
            else
            {
                Discounts = discounts.ToList();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _discountService.DeleteDiscountAsync(id);
                TempData["SuccessMessage"] = "Xóa chương trình giảm giá thành công";
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Xảy ra lỗi khi xóa chương trình giảm giá: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(int id, bool isActive)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);
                discount.IsActive = !isActive;

                await _discountService.UpdateDiscountAsync(id, discount);

                TempData["SuccessMessage"] = $"Đã {(isActive ? "tắt" : "bật")} chương trình giảm giá thành công";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Xảy ra lỗi khi cập nhật trạng thái: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}