﻿using Domain.DTO;

namespace Domain.Services
{
    public interface IProductService
    {
        public Task<bool> AddProductAsync(ProductDto product);
        public Task<List<ProductDto>> GetListProductsAsync();
        public Task<bool> UpdateProductAsync(ProductDto productDto);
        public Task<bool> DeleteProductAsync(Guid id);
        public Task<ProductDto> GetProductDtoByIdAsync(Guid id);
    }
}
