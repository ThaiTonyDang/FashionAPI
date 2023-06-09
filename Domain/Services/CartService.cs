using Domain.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<Tuple<bool, string>> SaveCartAsyn(CartItemDto cartDto)
        {
            var cartItem = new CartItem()
            {
                CartId = cartDto.CartId,
                Quantity = cartDto.Quantity,
                UserId = cartDto.UserId,
                Product = new Product
                {
                    Id = cartDto.ProductDto.Id,
                    Name = cartDto.ProductDto.Name,
                    Price = cartDto.ProductDto.Price,
                    Provider = cartDto.ProductDto.Provider,
                    CategoryId = cartDto.ProductDto.CategoryId,
                    Description = cartDto.ProductDto.Description,
                    CreatedDate = cartDto.ProductDto.CreateDate,
                    ModifiedDate= cartDto.ProductDto.ModifiedDate,
                    QuantityInStock = cartDto.ProductDto.QuantityInStock,                 
                }
            };
            var result = await _cartRepository.SaveCartAsyn(cartItem);
            return Tuple.Create(result.Item1, result.Item2);
        }
    }
}
