using Microsoft.AspNetCore.Mvc;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly ILogger<CategoryController> _categoryLogger;

        public CategoryController(ICategoryRepo categoryRepo, ILogger<CategoryController> categoryLogger)
        {
            _categoryRepo = categoryRepo;
            _categoryLogger = categoryLogger;
        }

        [HttpPost("/add-category"), Authorize(Roles = "admin")]
        public IActionResult AddCategory(CategoryDto category)
        {
            if (_categoryRepo.CategoryExists(category.Name))
                return StatusCode(409, "Category Exists");

            Category newCategory = new Category();
            newCategory.Name = category.Name;
            if (_categoryRepo.AddCategory(newCategory))
                return Ok("Category Added successfully");

            return BadRequest("Error in Adding Category");
        }

        [HttpDelete("/delete-category/{categoryName}"), Authorize(Roles = "admin")]
        public IActionResult DeleteCategory(string categoryName)
        {
            if (!_categoryRepo.CategoryExists(categoryName))
                return NotFound("Category Not Found");

            if (_categoryRepo.DeleteCategory(categoryName))
                return Ok("Deleted successfully");

            return BadRequest("Error in deleting Category");
        }

        [HttpGet("/category/{categoryName}")]
        public IActionResult GetProducts(string categoryName, int pageNumber=1, int pageSize=10) 
        {
            if (!_categoryRepo.CategoryExists(categoryName))
                return NotFound("Category Not Found");

            PaginatedList<Product> products = _categoryRepo.GetProducts(categoryName, pageNumber, pageSize);
            _categoryLogger.LogInformation("Fetched products successfully. {@products}", products);
            return Ok(products);    
        }

        [HttpGet("/categories")]
        public IActionResult GetCategories()
        {
            IEnumerable<Category> categories = _categoryRepo.GetCategories();
            return Ok(categories);
        }

    }
}
