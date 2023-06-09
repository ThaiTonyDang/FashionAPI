using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ICartRepository
    {
        public Task<Tuple<bool, string>> SaveCartAsyn(CartItem cartItems);
        public void AddToCart(Product product, List<CartItem> carts, int quantityInput);
        public bool DeleteCartItems(List<CartItem> carts, Guid id);
        public void AdjustQuantity(CartItem cartItem, string operate);
    }
}
