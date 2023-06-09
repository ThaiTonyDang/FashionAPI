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
        public void AddToCart(Product product, List<CartItem> carts, int quantityInput)
        {
            throw new NotImplementedException();
        }

        public void AdjustQuantity(CartItem cartItem, string operate)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCartItems(List<CartItem> carts, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<bool, string>> SaveCartAsyn(CartItem cart)
        {
            var error = "";
            try
            {
                if (cart == null)
                    return Tuple.Create(false, "Cart Item Null! Create Cart Fail");
                
                if (cart.CartId == default(Guid))
                {
                    error = $"Id Invalid ! The product {cart.Product.Name} cannot be added !";
                    return Tuple.Create(false, error );
                };
                if (cart.Product.QuantityInStock == 0)
                {
                    error = $"The product {cart.Product.Name} out of stock !";
                    return Tuple.Create(false, error);
                };
                await _appDbContext.Carts.AddAsync(cart);
                var result = await _appDbContext.SaveChangesAsync();
                var message = "Save Cart Success !";
                    return Tuple.Create(result > 0, message);
            }
            catch (Exception e)
            {
                error =  e.Message ;
                return Tuple.Create(false, error);
            }
        }
    }
}
