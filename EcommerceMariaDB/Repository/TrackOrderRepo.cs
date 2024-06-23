using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;

namespace EcommerceMariaDB.Repository
{
    public class TrackOrderRepo : ITrackOrderRepo
    {
        private readonly DataContext _db;
        private readonly IEmailRepo _emailRepo;
        public TrackOrderRepo(DataContext db, IEmailRepo emailRepo)
        {
            _db = db;
            _emailRepo = emailRepo;
        }

        public bool CreateTrack(List<int> orderItemID)
        {
            foreach (int oi in orderItemID)
            {
                TrackOrder trackOrder = new TrackOrder();
                trackOrder.OrderItemId = oi;
                trackOrder.Ordered = true;
                _db.TrackOrders.Add(trackOrder);
            }
            return UtilsRepo.Save(_db);
        }

        public TrackOrder? GetStatus(int orderItemID)
        {
            TrackOrder? trackOrder = _db.TrackOrders.Where(to => to.OrderItemId == orderItemID).FirstOrDefault();
            if (trackOrder == null)
                return null;

            return trackOrder;
        }

        public bool UpdateStatus(int orderItemId, string status)
        {
            TrackOrder? trackOrder = _db.TrackOrders.Where(to => to.OrderItemId == orderItemId).FirstOrDefault();
            if (trackOrder == null)
                return false;

            if (status == "Shipped")
                trackOrder.Shipped = true;
            else if (status == "OutForDelivery")
                trackOrder.OutForDelivery = true;
            else if (status == "Delivered")
                trackOrder.Delivered = true;

            string userMail = GetUserEmail(orderItemId);
            Email email = new Email();
            email.To = userMail;
            email.Subject = $"Your order has been {status.ToUpper()}";
            email.Body = $"Order Details : \nID : {orderItemId} \nCurrent Status : {status}";
            _emailRepo.SendEmail(email);

            _db.Update(trackOrder);
            return UtilsRepo.Save(_db);
        }

        private string GetUserEmail(int orderItemId)
        {
            List<string> email = (from orderItem in _db.OrderItems
                                  where orderItem.Id == orderItemId
                                  join order in _db.Orders
                                  on orderItem.OrderId equals order.Id
                                  join user in _db.Users
                                  on order.UserId equals user.Id
                                  select user.Email).ToList();

            return email[0];
        }
    }
}
