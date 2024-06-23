using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Interfaces
{
    public interface ISellerRepo
    {
        Seller? GetSeller(int sellerId);
        bool UpgradeToSeller(SellerDto sellerDto, int userId);
        bool AddSellerProduct(int sellerId, int productId);
        bool DeleteSellerProduct(int sellerId, List<int> productId);
        IQueryable GetSellerProducts(int sellerId);
        bool AddSellerOrder(List<int> orderItemIds);
        IQueryable GetSellerOrders(int sellerId);
    }
}
