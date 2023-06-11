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

        public CartItemDto GetCartItemById(Guid userId, Guid productId)
        {
            var carto = _cartRepository.GetCartItemById(userId, productId);
            var cartDto = new CartItemDto
            {
                ProductId = carto.ProductId,
                Quantity = carto.Quantity,
            };

            return cartDto;
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
            var isExists = false;
            var cartItem = new Cart()
            {
                Quantity = cartDto.Quantity,
                ProductId = cartDto.ProductId,
                UserId = userId
            };

            var cartEntity = _cartRepository.GetCartItemById(userId, cartDto.ProductId);
            if (cartEntity != null)
            {
                isExists = true;
                var quantity = cartDto.Quantity;
                cartItem.Quantity = quantity + cartEntity.Quantity;
            }

            var result = await _cartRepository.SaveCartAsyn(cartItem, isExists);
            var isSuccess = result.Item1;
            var message = result.Item2;
            return Tuple.Create(isSuccess, message);
        }      
    }
}
