using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Repository
{
    public class CartRepo : ICartRepo
    {
        private readonly DataContext _db;

        public CartRepo(DataContext db)
        {
            _db = db;
        }

        public bool AddToCart(CartItemDto item, int userId)
        {
            Product pro = _db.Products.Find(item.ProductId);
            float proPrice = pro.Price;
            int cartId = GetCart(userId);

            CartItem? cartItem = ProductExistInCart(pro.Id, cartId);

            if (cartItem != null)
            {
                cartItem.Quantity += item.Quantity;
                cartItem.Amount = cartItem.Quantity * proPrice;

                _db.CartItems.Update(cartItem);
            }
            else
            {
                cartItem = new CartItem();
                cartItem.ProductId = item.ProductId;
                cartItem.Quantity = item.Quantity;
                cartItem.Amount = item.Quantity * proPrice;
                cartItem.CartId = cartId;
                _db.CartItems.Add(cartItem);
            }

            return UtilsRepo.Save(_db);
        }

        public bool RemoveFromCart(int productId, int userId)
        {
            int cartId = GetCart(userId);
            CartItem? item = _db.CartItems.Where(ci => ci.ProductId == productId && ci.CartId == cartId).FirstOrDefault();
            if (item == null)
                return false;
            _db.CartItems.Remove(item);
            return UtilsRepo.Save(_db);
        }

        public IQueryable GetCartItems(int userId)
        {
            int cartId = GetCart(userId);
            var items = from cartItem in _db.CartItems
                        where cartItem.CartId == cartId
                        join product in _db.Products
                        on cartItem.ProductId equals product.Id
                        select new
                        {
                            productId = product.Id,
                            productName = product.Title,
                            quantity = cartItem.Quantity,
                            amount = cartItem.Amount,
                        };

            return items;
        }

        private int GetCart(int userId)
        {
            Cart cart = _db.Carts.First(c => c.UserId == userId);
            return cart.Id;
        }

        private CartItem? ProductExistInCart(int productId, int cartId)
        {
            CartItem? cartItem = _db.CartItems.Where(ci => ci.ProductId == productId && ci.CartId == cartId).FirstOrDefault();
            return cartItem;
        }
    }
}
