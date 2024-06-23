using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMariaDB.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly DataContext _db;
        private readonly ISellerRepo _sellerRepo;
        public CategoryRepo(DataContext db, ISellerRepo sellerRepo)
        {
            _db = db;
            _sellerRepo = sellerRepo;
        }

        public bool CategoryExists(string categoryName)
        {
            return _db.Categories.Any(c => c.Name == categoryName);
        }

        public bool AddCategory(Category category)
        {
            _db.Categories.Add(category);
            return UtilsRepo.Save(_db);
        }

        public bool DeleteCategory(string categoryName)
        {
            Category? category = _db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();

            // Remove seller products
            var sellerProducts = (from pro in _db.Products
                                  group pro by pro.SellerId
                                  into sellerPro
                                  select sellerPro
                                 ).ToList();

            foreach (var seller in sellerProducts)
            {
                List<int> proIds = (from pro in seller
                                    select pro.Id).ToList();
                _sellerRepo.DeleteSellerProduct(seller.Key, proIds);
            }

            // Then remove category
            _db.Categories.Remove(category);
            return UtilsRepo.Save(_db);
        }

        public PaginatedList<Product> GetProducts(string categoryName, int pageNumber, int pageSize)
        {
            //Category? category = _db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
            //ICollection<Product> products = category.Products;
            IQueryable<Product> products = from product in _db.Products
                                           join category in _db.Categories
                                           on product.CategoryId equals category.Id
                                           where category.Name == categoryName
                                           select product;


            return PaginatedList<Product>.Create(products, pageNumber, pageSize);
        }

        public IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> categories = _db.Categories;
            return categories;
        }
    }
}
