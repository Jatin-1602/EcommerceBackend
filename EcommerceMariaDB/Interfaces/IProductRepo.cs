using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Interfaces
{
    public interface IProductRepo
    {
        bool ProductExists(int productId);
        bool ProductExistsForSeller(int sellerId, string title);
        bool AddProduct(ProductDto product);
        bool DeleteProduct(int productId);
        bool? UpdateProduct(int productId, ProductDto product);
        Product? GetProduct(int productId);
        IQueryable<Product> GetProducts(string search, float? minPrice, float? maxPrice);
    }
}
