using SmartShop.Core.DTOs;
using SmartShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShop.Infrastructure.Services
{
    class ProductService : IProductService
    {
        public Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> UpdateProductAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
