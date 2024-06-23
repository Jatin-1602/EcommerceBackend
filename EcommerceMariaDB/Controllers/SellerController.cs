using EcommerceMariaDB.Repository;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController: Controller
    {
        private readonly ISellerRepo _sellerRepo;
        public SellerController(ISellerRepo sellerRepo)
        {
            _sellerRepo = sellerRepo;
        }

        [HttpPost("/upgrade-to-seller"), Authorize]
        public IActionResult UpgradeToSeller(SellerDto seller) 
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");

            int userId = UtilsRepo.GetDetails(authHeader);

            if (_sellerRepo.UpgradeToSeller(seller, userId))
                return Ok("Upgraded to Seller successfully");

            return BadRequest("Error in saving Seller");
        }

        [HttpGet("/seller-products"), Authorize(Roles = "seller")]
        public IActionResult GetSellerProducts(int sellerId)
        {
            if (_sellerRepo.GetSeller(sellerId) == null)
                return NotFound("Seller not exists");

            var sellerPro = _sellerRepo.GetSellerProducts(sellerId);
            return Ok(sellerPro);
        }

        [HttpGet("/seller-orders"), Authorize(Roles = "seller")]
        public IActionResult GetSellerOrders(int sellerId)
        {
            if (_sellerRepo.GetSeller(sellerId) == null)
                return NotFound("Seller not exists");

            var sellerOrders = _sellerRepo.GetSellerOrders(sellerId);
            return Ok(sellerOrders);
        }
    }
}
