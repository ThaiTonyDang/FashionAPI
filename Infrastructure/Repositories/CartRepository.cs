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

        public async Task<Tuple<bool, string>> DeleteCartItemAsync(Guid userId, Guid productId)
        {
            var cartItem = GetCartItemById(userId, productId);
            if (cartItem == null)
            {
                return Tuple.Create(false, "Cart item not found! Delete fail !");
            }

            _appDbContext.CartItems.Remove(cartItem);
            var result = await _appDbContext.SaveChangesAsync();
            return Tuple.Create(true, "Deleted success !");
        }

        public Cart GetCartItemById(Guid userId, Guid productId)
        {
            var cartEntity = _appDbContext.CartItems.Where(c => c.ProductId == productId && c.UserId == userId)
                                                       .FirstOrDefault();
            return cartEntity;
        }

        public Task<List<Cart>> GetCartItemsAsync(Guid userId)
        {
            var cartItems = _appDbContext.CartItems.ToList().FindAll(c => c.UserId == userId);
            var products = _appDbContext.Products.ToList();
            var cart = (from p in products
                        join c in cartItems on p.Id equals c.ProductId
                        select new Cart
                        {
                            UserId = c.UserId,
                            Quantity = c.Quantity,
                            Product = new Product
                            {
                                Name = p.Name,
                                Price = p.Price,
                                CategoryId = p.CategoryId,
                                CreatedDate = p.CreatedDate,
                                ModifiedDate = p.ModifiedDate,
                                MainImageName = p.MainImageName,
                                Provider = p.Provider,
                                Description = p.Description
                            }                          
                        }).ToList();

            if (cartItems == null) return Task.FromResult(new List<Cart>());
            return Task.FromResult(cartItems);
        }

        public async Task<Tuple<bool, string>> SaveCartAsyn(Cart cart)
        {
            var message = "";
            var result = 0;

            if (cart == null)
                return Tuple.Create(false, "Cart Item Null! Create Cart Fail");
               
            var cartEntity = GetCartItemById(cart.UserId, cart.ProductId);
            if (cartEntity != null)
            {
                cartEntity.Quantity = cart.Quantity + cartEntity.Quantity;
                _appDbContext.Update(cartEntity);
                result = await _appDbContext.SaveChangesAsync();
                return Tuple.Create(result > 0, "Save Success !");
            }

            await _appDbContext.CartItems.AddAsync(cart);
            result = await _appDbContext.SaveChangesAsync();
            message = "Save Cart Success !";
            return Tuple.Create(result > 0, message);          
        }

        public async Task<bool> UpdateQuantityCartItem(Cart cart)
        {
            var cartEntity = GetCartItemById(cart.UserId, cart.ProductId);
            if (cartEntity != null)
            {
                cartEntity.Quantity = cart.Quantity;
                _appDbContext.Update(cartEntity);
                var result = await _appDbContext.SaveChangesAsync();
                return (result > 0);
            }

            return false;
        }
    }
}
