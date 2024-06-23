using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Repository
{
    public class SellerRepo : ISellerRepo
    {
        private readonly DataContext _db;
        public SellerRepo(DataContext db)
        {
            _db = db;
        }

        public Seller? GetSeller(int sellerId)
        {
            return _db.Sellers.Find(sellerId);
        }

        public bool UpgradeToSeller(SellerDto sellerDto, int userId)
        {
            Seller seller = new Seller();
            seller.Name = sellerDto.Name;
            seller.GST = sellerDto.GST;
            seller.Mobile = sellerDto.Mobile;
            seller.PinCode = sellerDto.PinCode;
            seller.Address = sellerDto.Address;
            seller.UserId = userId;

            _db.Sellers.Add(seller);

            //Update user role
            User user = _db.Users.Find(userId);
            user.Role = "seller";
            _db.Users.Update(user);

            return UtilsRepo.Save(_db);
        }

        public IQueryable GetSellerOrders(int sellerId)
        {
            var orders = from so in _db.SellerOrders
                         where so.SellerId == sellerId
                         join oi in _db.OrderItems
                         on so.OrderItemId equals oi.Id
                         join pro in _db.Products
                         on oi.ProductId equals pro.Id
                         select new
                         {
                             OrderItemId = oi.Id,
                             ProductTitle = pro.Title,
                             Quantiy = oi.Quantity,
                             Amount = oi.Price,
                             Seller = so.SellerId,
                         };

            return orders;
        }

        public int GetSellerIdFromOrderItem(int orderItemId)
        {
            int sellerId = (from oi in _db.OrderItems
                           where oi.Id == orderItemId
                           join pro in _db.Products
                           on oi.ProductId equals pro.Id
                           select pro.SellerId).ToList()[0];
            return sellerId;
        }

        public bool AddSellerOrder(List<int> orderItemIds)
        {
            foreach (int oid in orderItemIds)
            {
                int sellerId = GetSellerIdFromOrderItem(oid);
                SellerOrder sellerOrder = new SellerOrder();
                sellerOrder.OrderItemId = oid;
                sellerOrder.SellerId = sellerId;
                _db.SellerOrders.Add(sellerOrder);
            }

            return UtilsRepo.Save(_db);
        }


        public IQueryable GetSellerProducts(int sellerId)
        {
            var groupByCategory = from pro in _db.Products
                                  where pro.SellerId == sellerId
                                  group pro by pro.CategoryId
                                   into sellerPro
                                  orderby sellerPro.Key
                                  select new 
                                  {
                                      CategoryId = sellerPro.Key,
                                      Products = sellerPro.ToList()
                                  };

            //foreach (var sellerGroup in groupByCategory)
            //{
            //    Console.WriteLine($"Key: {sellerGroup.CategoryId}, {sellerGroup.Products}");
            //    foreach (var pro in sellerGroup.Products)
            //    {
            //        Console.WriteLine($"{pro.Title} {pro.Price}");
            //    }
                
            //}

            return groupByCategory;
        }

        public bool AddSellerProduct(int sellerId, int productId)
        {
            SellerProduct sellerProduct = new SellerProduct();
            sellerProduct.ProductId = productId;
            sellerProduct.SellerId = sellerId;
            _db.SellerProducts.Add(sellerProduct);
            return UtilsRepo.Save(_db);
        }

        public bool DeleteSellerProduct(int sellerId, List<int> productIds)
        {
            SellerProduct? sellerProduct = null;
            foreach (int productId in productIds)
            {
                sellerProduct = _db.SellerProducts.Where(sp => sp.ProductId == productId && sp.SellerId == sellerId).FirstOrDefault();
                if (sellerProduct != null)
                    _db.SellerProducts.Remove(sellerProduct);
            }
            return UtilsRepo.Save(_db);
        }
    }
}
