using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface ICartRepository
    {
        public Task<Tuple<bool, string>> SaveCartAsyn(Cart cartItems);
        public List<Cart> GetCartItems(Guid userId);
    }
}
