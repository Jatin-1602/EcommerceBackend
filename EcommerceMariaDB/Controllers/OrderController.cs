using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        public OrderController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        private (int, string) GetDetails(string authHeader)
        {
            var _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = _jwtSecurityTokenHandler.ReadToken(authHeader);
            Console.WriteLine(jsonToken);

            var tokenS = _jwtSecurityTokenHandler.ReadToken(authHeader) as JwtSecurityToken;
            Console.WriteLine(tokenS);
            int userId = Int32.Parse(tokenS.Claims.First(c => c.Type == "id").Value);
            string email = tokenS.Claims.First(c => c.Type == "email").Value;
            Console.WriteLine($"User id = {userId}");

            return (userId, email);
        }

        [HttpPost("/place-order"), Authorize]
        public IActionResult PlaceOrder(OrderDto details)
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");
            //Console.WriteLine(authHeader);

            (int userId, string email) = GetDetails(authHeader);

            if (_orderRepo.AddOrder(details, userId))
            {
                return Ok("Order successfully placed");
            }

            return BadRequest("Error in placing order");
        }

        [HttpGet("/orders")]
        public IActionResult GetOrders()
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");
            //Console.WriteLine(authHeader);

            (int userId, string email) = GetDetails(authHeader);

            IQueryable<Order> orders = _orderRepo.GetOrders(userId);
            return Ok(orders);
        }

        [HttpGet("/orders/{orderId}")]
        public IActionResult GetOrderReceipt(int orderId)
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");
            //Console.WriteLine(authHeader);

            (int userId, string email) = GetDetails(authHeader);
            var items = _orderRepo.GetOrderDetails(orderId, userId);
            
            if (items == null)
                return BadRequest("Invalid Order ID");

            return Ok(items);
        }
    }

}
