using Microsoft.AspNetCore.Mvc;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceMariaDB.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpPost("/add-product"), Authorize(Roles = "admin,seller")]
        public IActionResult AddProduct(ProductDto pro)
        {
            if (pro.ForceAdd != true && _productRepo.ProductExistsForSeller(pro.SellerId, pro.Title))
                return StatusCode(409, "Product already Exists");

            if (_productRepo.AddProduct(pro))
                return Ok("Product added successfully");

            return BadRequest("Error in adding product");
        }

        [HttpDelete("/delete-product/{productId}"), Authorize(Roles = "admin,seller")]
        public IActionResult DeleteProduct(int productId) 
        {
            if (!_productRepo.ProductExists(productId))
                return NotFound("Product Not Found");

            if (_productRepo.DeleteProduct(productId))
                return Ok("Deleted Successfully");

            return BadRequest("Error in deleting product");
        }

        [HttpGet("/products/{productId}")]
        public IActionResult GetProduct(int productId)
        {
            if (!_productRepo.ProductExists(productId))
                return NotFound("Product Not Found");

            Product? product = _productRepo.GetProduct(productId);
            return Ok(product);
        }

        [HttpGet("/get-products/{search}")]
        public IActionResult GetProducts(string search, float? minPrice, float? maxPrice)
        {
            IQueryable<Product> products = _productRepo.GetProducts(search, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpPut("/update-product")]
        public IActionResult UpdateProduct(int productId, ProductDto pro)
        {
            bool? response = _productRepo.UpdateProduct(productId, pro);
            if (response == null)
                return NotFound("Product Not Found");
            else if (response == true)
                return Ok("Product updated successfully");

            return BadRequest("Error in updating product");
        }
    }
}
