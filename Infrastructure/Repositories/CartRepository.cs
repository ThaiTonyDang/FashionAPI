using Infrastructure.DataContext;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;
        public CartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Cart GetCartItemById(Guid userId, Guid productId)
        {
            var cartEntity = _appDbContext.CartItems.Where(c => c.ProductId == productId && c.UserId == userId)
                                                       .FirstOrDefault();
            return cartEntity;
        }

        public List<Cart> GetCartItems(Guid userId)
        {
            var cartItems = _appDbContext.CartItems.ToList().FindAll(c => c.UserId == userId);
            if (cartItems == null) return new List<Cart>();
            return cartItems;
        }

        public async Task<Tuple<bool, string>> SaveCartAsyn(Cart cart, bool isExists)
        {
            var message = "";
            var result = 0;

            if (cart == null)
                return Tuple.Create(false, "Cart Item Null! Create Cart Fail");
               
            if(isExists)
            {
                var cartEntity = GetCartItemById(cart.UserId, cart.ProductId);
                cartEntity.Quantity = cart.Quantity;
                _appDbContext.Update(cartEntity);
                result = await _appDbContext.SaveChangesAsync();
                return Tuple.Create(result > 0, "Save Success !");
            }

            await _appDbContext.CartItems.AddAsync(cart);
            result = await _appDbContext.SaveChangesAsync();
            message = "Save Cart Success !";
            return Tuple.Create(result > 0, message);
            
        }
    }
}
