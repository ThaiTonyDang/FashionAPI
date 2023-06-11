using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface ICartRepository
    {
        public Task<Tuple<bool, string>> SaveCartAsyn(Cart cartItems, bool isExists);
        public List<Cart> GetCartItems(Guid userId);
        public Cart GetCartItemById(Guid userId, Guid productId);
    }
}
