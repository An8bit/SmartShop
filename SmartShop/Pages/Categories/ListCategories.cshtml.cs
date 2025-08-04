using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;



namespace SmartShop.Web.Pages.Categories
{
    public class ListCategoriesModel(ICategoryService categoryService) : PageModel
    {
        private readonly ICategoryService _categoryService = categoryService;

      
        public IList<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

       

        public async Task OnGetAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            Categories = categories.ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            { 
                await _categoryService.DeleteCategoryAsync(id);
                TempData["SuccessMessage"] = "Xóa danh mục thành công";
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch
            {
                TempData["ErrorMessage"] = "Xảy ra lỗi khi xóa danh mục";
            }

            return RedirectToPage();
        }
    }
}