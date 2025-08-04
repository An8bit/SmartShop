using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartShop.Infrastructure.ApiClients;

namespace SmartShop.Infrastructure.Services
{
    public class CategoriesService : ICategoryService
    {
        private readonly IApiClient _apiClient;

        public CategoriesService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoriesCreateDto categoryCreateDto)
        {
            try
            {
                return await _apiClient.PostAsync<CategoryDto>("api/Categories", categoryCreateDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating category: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Categories/{id}");
                return true; // Assuming successful deletion if no exception
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                
                var categories = await _apiClient.GetAsync<IEnumerable<CategoryDto>>("api/Categories");
                
                return categories ?? new List<CategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting categories from API: {ex.Message}");

                // Return default categories if API fails
                return new List<CategoryDto>
                {
                    new CategoryDto { CategoryId = 1, CategoryName = "Nam" },
                    new CategoryDto { CategoryId = 2, CategoryName = "Nữ" },
                    
                };
            }
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _apiClient.GetAsync<CategoryDto>($"api/Categories/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting category by id: {ex.Message}", ex);
            }
        }

        public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            try
            {
                return await _apiClient.PutAsync<CategoryDto>($"api/Categories/{id}", categoryDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating category: {ex.Message}", ex);
            }
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _apiClient.GetAsync<List<CategoryDto>>("api/Categories");
                return categories ?? new List<CategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting categories from API: {ex.Message}");

              
                return new List<CategoryDto>
                {
                    new CategoryDto { CategoryId = 1, CategoryName = "Nam" },
                    new CategoryDto { CategoryId = 2, CategoryName = "Nữ" },
                   
                };
            }
        }
    }
}