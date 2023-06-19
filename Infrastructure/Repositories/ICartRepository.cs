using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface ICartRepository
    {
        public Task<Tuple<bool, string>> SaveCartAsyn(Cart cart);
        public Task<List<Cart>> GetCartItemsAsync(Guid userId);
        public Cart GetCartItemById(Guid userId, Guid productId);
        public Task<Tuple<bool, string>> DeleteCartItemAsync(Guid userId, Guid productId);
        public Task<bool> UpdateQuantityCartItem(Cart cart);
        public Task<bool> DeleteAllCart(Guid userId);
    }
}
