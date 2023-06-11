using Domain.Dtos;
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
        public Task<Tuple<bool, string>> SaveCartAsyn(CartItemDto cartDto, Guid userId);
        public List<CartItemDto> GetCartItems(Guid userId);
        public CartItemDto GetCartItemById(Guid userId, Guid productId);
    }
}
