using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Interfaces
{
    public interface IOrderRepo
    {
        bool AddOrder(OrderDto details, int userId);
        bool PlaceOrder(int orderId, int userId);
        IQueryable<Order> GetOrders(int userId);
        IQueryable? GetOrderDetails(int orderId, int userId);
    }
}
