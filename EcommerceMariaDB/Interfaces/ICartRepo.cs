using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Interfaces
{
    public interface ICartRepo
    {
        bool AddToCart(CartItemDto item, int userId);
        bool RemoveFromCart(int productId, int userId);
        IQueryable GetCartItems(int userId);
    }
}
