using Domain.Dtos;
using Domain.ViewModel;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ICartService
    {
        public Task<Tuple<bool, string>> SaveCartAsyn(CartItemDto cartDto);
        public Task<List<CartItemViewModel>> GetCartItems(Guid userId);
        public CartItemDto GetCartItemById(Guid userId, Guid productId);
        public Task<Tuple<bool, string>> DeleteCartItemAsync(Guid userId, Guid productId);
        public Task<bool> UpdateQuantityCartItem(CartItemDto cartDto);
        public Task<bool> DeleteAllCartAsyn(Guid userId);
    }
}
