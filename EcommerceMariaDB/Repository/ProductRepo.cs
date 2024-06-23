using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;
using System.Linq;

namespace EcommerceMariaDB.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly DataContext _db;
        private readonly ISellerRepo _sellerRepo;
        public ProductRepo(DataContext db, ISellerRepo sellerRepo)
        {
            _db = db;
            _sellerRepo = sellerRepo;
        }

        public bool ProductExists(int productId)
        {
            return _db.Products.Any(p => p.Id == productId);
        }

        public bool ProductExistsForSeller(int sellerId, string title)
        {
            return _db.Products.Any(p => p.SellerId == sellerId && p.Title.ToLower() == title.ToLower());
        }

        public bool AddProduct(ProductDto pro)
        {
            //Console.WriteLine(Request.Form.Files.FirstOrDefault().FileName);
            IFormFile file = pro.File;
            byte[] fileBytes = null;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
                Console.WriteLine($"{fileBytes} {fileBytes.Length}");
            }

            Product product = new Product();
            product.Title = pro.Title;
            product.Description = pro.Description;
            product.Price = pro.Price;
            product.CategoryId = pro.CategoryId;
            product.Image = fileBytes;
            if (pro.UnitCount != null)
                product.UnitCount += (int)pro.UnitCount;
            else
                product.UnitCount += 1;
            product.SellerId = pro.SellerId;

            _db.Products.Add(product);
            if (UtilsRepo.Save(_db))
            {
                return _sellerRepo.AddSellerProduct(product.SellerId, product.Id);
            }
            return false;
        }

        public bool DeleteProduct(int productId)
        {
            Product? product = GetProduct(productId);
            List<int> productIds = new List<int>();
            if (product != null)
            {
                productIds.Add(productId);
                if (_sellerRepo.DeleteSellerProduct(product.SellerId, productIds))
                {
                    _db.Products.Remove(product);
                    return UtilsRepo.Save(_db);
                }
            }
            return false;
        }

        public Product? GetProduct(int productId)
        {
            Product? product = _db.Products.FirstOrDefault(p => p.Id == productId);
            return product;
        }

        public bool? UpdateProduct(int productId, ProductDto pro)
        {
            Product? product = GetProduct(productId);
            if (product == null)
                return null;

            product.Title = pro.Title;
            product.Description = pro.Description;
            product.Price = pro.Price;
            product.CategoryId = pro.CategoryId;


            IFormFile? file = pro.File;
            if (file != null)
            {
                byte[]? fileBytes = null;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                    Console.WriteLine($"{fileBytes} {fileBytes.Length}");
                }

                product.Image = fileBytes;
            }

            product.ModifiedAt = DateTime.Now;

            _db.Products.Update(product);
            return UtilsRepo.Save(_db);
        }

        public IQueryable<Product> GetProducts(string search, float? minPrice, float? maxPrice)
        {
            IQueryable<Product> products = _db.Products.Where(p => p.Title.ToLower().Contains(search.ToLower()));
            if (minPrice != null)
                products = products.Where(p => p.Price >= minPrice);
            if (maxPrice != null)
                products = products.Where(p => p.Price <= maxPrice);

            return products;
        }
    }
}
