using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartShop.Infrastructure.ApiClients;
namespace SmartShop.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IApiClient _apiClient;

        public ProductService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                return await _apiClient.PostAsync<ProductDto>("api/Products", productDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating product: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Products/{id}");
                return true; // Assuming successful deletion if no exception
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                return await _apiClient.GetAsync<IEnumerable<ProductDto>>("api/Products/all");
            }
            catch (Exception ex)
            {
                // Log exception nếu cần
                Console.WriteLine($"Error calling API: {ex.Message}");
                
                // Trả về empty list thay vì mock data để có thể debug API issue
                return new List<ProductDto>();
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            return await _apiClient.GetAsync<ProductDto>("api/Products/"+id);
        }

        public Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
        {
           try
          {
               return await _apiClient.PutAsync<ProductDto>($"api/Products/{productDto.ProductId}", productDto);
            }
            catch (Exception ex)
            {
             throw new Exception($"Error updating product: {ex.Message}", ex);
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

                // Return default categories if API fails
                return new List<CategoryDto>
                {
                    new CategoryDto { CategoryId = 1, CategoryName = "Nam" },
                    new CategoryDto { CategoryId = 2, CategoryName = "Nữ" },
                    new CategoryDto { CategoryId = 3, CategoryName = "Unisex" },
                    new CategoryDto { CategoryId = 4, CategoryName = "Phụ kiện" }
                };
            }
        }
    }
}
