using EcommerceMariaDB.Models;

namespace EcommerceMariaDB.Interfaces
{
    public interface ICategoryRepo
    {
        bool CategoryExists(string categoryName);
        bool AddCategory(Category category);
        bool DeleteCategory(string categoryName);
        PaginatedList<Product> GetProducts(string categoryName, int pageNumber, int pageSize);

        IEnumerable<Category> GetCategories();
    }
}
