using Domain.Dtos;
using Domain.ViewModel;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> DeleteAllCartAsyn(Guid userId)
        {
            var result = await _cartRepository.DeleteAllCart(userId);
            return result;
        }

        public async Task<Tuple<bool, string>> DeleteCartItemAsync(Guid userId, Guid productId)
        {
            var result = await _cartRepository.DeleteCartItemAsync(userId, productId);
            var isSuccess = result.Item1;
            var message = result.Item2;

            return Tuple.Create(isSuccess, message);
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

        public async Task<List<CartItemViewModel>> GetCartItems(Guid userId)
        {
            var carts = await _cartRepository.GetCartItemsAsync(userId);
            var cartDto = carts.Select(c => new CartItemViewModel
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                UserId = c.UserId,
                Product = new ProductDto
                {
                    Name = c.Product.Name,
                    Price = c.Product.Price,
                    CategoryId = c.Product.CategoryId,
                    ModifiedDate = c.Product.ModifiedDate,
                    MainImageName = c.Product.MainImageName,
                    Provider = c.Product.Provider,
                    Description = c.Product.Description

                }
            }).ToList();
            return cartDto;
        }

        public async Task<Tuple<bool, string>> SaveCartAsyn(CartItemDto cartDto)
        {
            var cartItem = new Cart()
            {
                Quantity = cartDto.Quantity,
                ProductId = cartDto.ProductId,
                UserId = cartDto.UserId
            };

            var result = await _cartRepository.SaveCartAsyn(cartItem);
            var isSuccess = result.Item1;
            var message = result.Item2;
            return Tuple.Create(isSuccess, message);
        }

        public async Task<bool> UpdateQuantityCartItem(CartItemDto cartDto)
        {
            var cart = new Cart
            {
                ProductId = cartDto.ProductId,
                UserId = cartDto.UserId,
                Quantity = cartDto.Quantity         
            };
            var result = await _cartRepository.UpdateQuantityCartItem(cart);
            return result;
        }
    }
}
