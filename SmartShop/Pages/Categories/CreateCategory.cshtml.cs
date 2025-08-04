using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;

namespace SmartWeb.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public CategoriesCreateDto? Category { get; set; }

        public List<SelectListItem>? ParentCategories { get; set; }

        public async Task OnGetAsync()
        {
            await LoadParentCategoriesAsync();
        }

        private async Task LoadParentCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ParentCategories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadParentCategoriesAsync();
                return Page();
            }

            try
            {
                await _categoryService.CreateCategoryAsync(Category!);
                TempData["SuccessMessage"] = "Thêm danh mục thành công!";
                return RedirectToPage("/Categories/ListCategories");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi thêm danh mục: {ex.Message}";
                await LoadParentCategoriesAsync();
                return Page();
            }
        }
    }
}
