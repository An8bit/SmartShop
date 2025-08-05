using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartShop.Infrastructure.ApiClients;

namespace SmartShop.Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IApiClient _apiClient;

        public DiscountService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<DiscountDto> CreateDiscountAsync(DiscountCreateDto discountCreateDto)
        {
            try
            {
                return await _apiClient.PostAsync<DiscountDto>("api/Discounts", discountCreateDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating discount: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteDiscountAsync(int id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Discounts/{id}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync()
        {
            try
            {
                var discounts = await _apiClient.GetAsync<IEnumerable<DiscountDto>>("api/Discounts");
                return discounts ?? new List<DiscountDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting discounts from API: {ex.Message}");

                // Return default discounts if API fails
                return new List<DiscountDto>
                {
                    new DiscountDto {
                        DiscountId = 1,
                        ProductId = 1,
                        ProductName = "Sample Product 1",
                        DiscountPercentage = 10,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(7),
                        IsActive = true
                    },
                    new DiscountDto {
                        DiscountId = 2,
                        ProductId = 2,
                        ProductName = "Sample Product 2",
                        DiscountPercentage = 15,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(14),
                        IsActive = true
                    }
                };
            }
        }

        public async Task<DiscountDto> GetDiscountByIdAsync(int id)
        {
            try
            {
                return await _apiClient.GetAsync<DiscountDto>($"api/Discounts/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting discount by id: {ex.Message}", ex);
            }
        }

        public async Task<DiscountDto> UpdateDiscountAsync(int id, DiscountDto discountDto)
        {
            try
            {
                return await _apiClient.PutAsync<DiscountDto>($"api/Discounts/{id}", discountDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating discount: {ex.Message}", ex);
            }
        }

        public async Task<List<DiscountDto>> GetActiveDiscountsAsync()
        {
            try
            {
                var discounts = await _apiClient.GetAsync<List<DiscountDto>>("api/Discounts/active");
                return discounts ?? new List<DiscountDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active discounts: {ex.Message}");
                return new List<DiscountDto>();
            }
        }

        public async Task<List<DiscountDto>> GetProductDiscountsAsync(int productId)
        {
            try
            {
                return await _apiClient.GetAsync<List<DiscountDto>>($"api/Discounts/product/{productId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting discounts for product: {ex.Message}");
                return new List<DiscountDto>();
            }
        }
    }
}