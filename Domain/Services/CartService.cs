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

        public List<CartItemDto> GetCartItems(Guid userId)
        {
            var carts = _cartRepository.GetCartItems(userId).Select(c => new CartItemDto
            {
                Quantity = c.Quantity,
                ProductId = c.ProductId,
            }).ToList();

            return carts;
        }

        public async Task<Tuple<bool, string>> SaveCartAsyn(CartItemDto cartDto, Guid userId)
        {
            var cartItem = new Cart()
            {
                Quantity = cartDto.Quantity,
                ProductId = cartDto.ProductId,
                UserId = userId
            };
            var result = await _cartRepository.SaveCartAsyn(cartItem);
            return Tuple.Create(result.Item1, result.Item2);
        }
    }
}
