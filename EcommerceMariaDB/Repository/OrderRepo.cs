using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceMariaDB.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DataContext _db;
        private readonly ITrackOrderRepo _trackOrderRepo;
        private readonly IEmailRepo _emailRepo;
        private readonly ISellerRepo _sellerRepo;
        public OrderRepo(DataContext db, ITrackOrderRepo trackOrderRepo, IEmailRepo emailRepo, ISellerRepo sellerRepo)
        {
            _db = db;
            _trackOrderRepo = trackOrderRepo;
            _emailRepo = emailRepo;
            _sellerRepo = sellerRepo;
        }

        private bool OrderExists(int orderId, int userId)
        {
            return _db.Orders.Any(o => o.Id == orderId && o.UserId == userId);
        }

        private IQueryable GetCartItems(int userId)
        {
            IQueryable items = from cart in _db.Carts
                               where cart.UserId == userId
                               join cartItem in _db.CartItems
                               on cart.Id equals cartItem.CartId
                               select new
                               {
                                   productId = cartItem.ProductId,
                                   quantity = cartItem.Quantity,
                                   price = cartItem.Amount,
                               };

            return items;
        }

        private bool RemoveItemsFromCart(int userId)
        {
            IQueryable<CartItem> items = from cart in _db.Carts
                                         where cart.UserId == userId
                                         join cartItem in _db.CartItems
                                         on cart.Id equals cartItem.CartId
                                         select cartItem;

            foreach (CartItem item in items)
            {
                _db.CartItems.Remove(item);
            }

            return UtilsRepo.Save(_db);
        }

        public bool AddOrder(OrderDto details, int userId)
        {
            Order order = new Order();
            order.City = details.City;
            order.State = details.State;
            order.PinCode = details.PinCode;
            order.Contact = details.Contact;
            order.UserId = userId;
            order.RazorpayPaymentId = details.RazorpayPaymentId;

            _db.Orders.Add(order);

            if (!UtilsRepo.Save(_db))
                return false;

            if (PlaceOrder(order.Id, order.UserId))
            {
                // Empty the cart of the user
                RemoveItemsFromCart(userId);
                string userMail = _db.Users.Find(order.UserId).Email;
                Email email = new Email();
                email.To = userMail;
                email.Subject = "Order has been successfully placed";
                email.Body = "Order has been successfully placed";
                _emailRepo.SendEmail(email);
                return true;
            }

            return false;
        }

        public bool PlaceOrder(int orderId, int userId)
        {
            Console.WriteLine($"Inside Place Order, Order id = {orderId}");
            var items = GetCartItems(userId);

            //float amount = 0;
            foreach (dynamic item in items)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.ProductId = item.productId;
                orderItem.Quantity = item.quantity;
                orderItem.Price = item.price;
                orderItem.OrderId = orderId;

                _db.OrderItems.Add(orderItem);
            }

            if (!UtilsRepo.Save(_db))
                return false;


            List<int> orderItemIds = (from orderItem in _db.OrderItems
                                      where orderItem.OrderId == orderId
                                      select orderItem.Id).ToList();

            
            _sellerRepo.AddSellerOrder(orderItemIds);

            return _trackOrderRepo.CreateTrack(orderItemIds);
        }

        public IQueryable<Order> GetOrders(int userId)
        {
            IQueryable<Order> orders = from order in _db.Orders
                                       where order.UserId == userId
                                       select order;
            return orders;
        }

        public IQueryable? GetOrderDetails(int orderId, int userId)
        {
            if (!OrderExists(orderId, userId))
                return null;

            IQueryable items = from orderItem in _db.OrderItems
                               where orderItem.OrderId == orderId
                               join product in _db.Products
                               on orderItem.ProductId equals product.Id
                               select new
                               {
                                   productTitle = product.Title,
                                   unitPrice = product.Price,
                                   quantity = orderItem.Quantity,
                                   amount = product.Price * orderItem.Quantity,
                               };

            return items;
        }


    }
}
