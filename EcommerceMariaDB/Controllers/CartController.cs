using Microsoft.AspNetCore.Mvc;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceMariaDB.Models.Dto;
using EcommerceMariaDB.Repository;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepo _cartRepo;
        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpGet("/cart")]
        public IActionResult GetCartItems() 
        {
            //string authHeader = Request.Headers.Authorization;
            //authHeader = authHeader.Replace("Bearer ", "");
            //Console.WriteLine(authHeader);

            //int userId = UtilsRepo.GetDetails(authHeader);
            
            int userId = int.Parse(HttpContext.Items["userId"]?.ToString()!);
            IQueryable items = _cartRepo.GetCartItems(userId);

            return Ok(items);
        }

        [HttpPost("/add-to-cart")]
        public IActionResult AddToCart(CartItemDto item)
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");
            
            int userId = UtilsRepo.GetDetails(authHeader);

            if (_cartRepo.AddToCart(item, userId))
                return Ok("Successfully added to cart");
            
            return BadRequest("Error in adding to cart");
        }

        [HttpDelete("/remove-from-cart/{productId}")]
        public IActionResult RemoveFromCart(int productId) 
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");

            int userId = UtilsRepo.GetDetails(authHeader);

            if (_cartRepo.RemoveFromCart(productId, userId))
                return Ok("Removed from Cart");

            return BadRequest("Error in removing from cart");
        }
    }
}
