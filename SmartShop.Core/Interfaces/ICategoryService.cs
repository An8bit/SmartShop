using SmartShop.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartShop.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CategoriesCreateDto categoryCreateDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryDto);
        Task<List<CategoryDto>> GetCategoriesAsync();
    }
}