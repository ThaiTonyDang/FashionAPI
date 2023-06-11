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

        public List<Cart> GetCartItems(Guid userId)
        {
            var cartItems = _appDbContext.CartItems.ToList().FindAll(c => c.UserId == userId);
            if (cartItems == null) return new List<Cart>();
            return cartItems;
        }

        public async Task<Tuple<bool, string>> SaveCartAsyn(Cart cart)
        {
            var message = "";
            var result = 0;
            try
            {
                if (cart == null)
                    return Tuple.Create(false, "Cart Item Null! Create Cart Fail");
               
                var cartEntity = _appDbContext.CartItems.Where(c => c.ProductId == cart.ProductId && c.UserId == cart.UserId)
                                                        .FirstOrDefault();
                if(cartEntity != null)
                {
                    var quantity = cart.Quantity;
                    cartEntity.Quantity = quantity + cartEntity.Quantity;
                    _appDbContext.Update(cartEntity);
                    result = await _appDbContext.SaveChangesAsync();
                    return Tuple.Create(result > 0, "Save Success !");
                }
                var product = _appDbContext.Products.Where(p => p.Id == cart.ProductId).FirstOrDefault();
                if (product == null) return Tuple.Create(false, "Product cannot found !");
                await _appDbContext.CartItems.AddAsync(cart);
                result = await _appDbContext.SaveChangesAsync();
                message = "Save Cart Success !";
                    return Tuple.Create(result > 0, message);
            }
            catch (Exception e)
            {
                return Tuple.Create(false, e.Message);
            }
        }
    }
}
