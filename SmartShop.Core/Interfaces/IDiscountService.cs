using SmartShop.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShop.Core.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountDto> CreateDiscountAsync(DiscountCreateDto discountCreateDto);
        Task<bool> DeleteDiscountAsync(int id);
        Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync();
        Task<DiscountDto> GetDiscountByIdAsync(int id);
        Task<DiscountDto> UpdateDiscountAsync(int id, DiscountDto discountDto);
        Task<List<DiscountDto>> GetActiveDiscountsAsync();
        Task<List<DiscountDto>> GetProductDiscountsAsync(int productId);
    }
}
