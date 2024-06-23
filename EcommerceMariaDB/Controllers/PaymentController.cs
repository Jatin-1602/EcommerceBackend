using EcommerceMariaDB.Repository;
using EcommerceMariaDB.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace ECommerceWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController: Controller
    {
        private readonly IPaymentRepo _paymentRepo;
        public PaymentController(IPaymentRepo paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [HttpGet("/checkout"), Authorize]
        public IActionResult Checkout()
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");

            int userId = UtilsRepo.GetDetails(authHeader);
            string razorpay_order_id = _paymentRepo.Checkout(userId);

            return Ok(razorpay_order_id);
        }

        [HttpPost("/verify-signature"), Authorize]
        public IActionResult VerifySignature(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            string authHeader = Request.Headers.Authorization;
            authHeader = authHeader.Replace("Bearer ", "");

            int userId = UtilsRepo.GetDetails(authHeader);

            if (_paymentRepo.VerifySignature(razorpay_payment_id, razorpay_order_id, razorpay_signature, userId))
                return Ok("Payment Successfull");

            return BadRequest("Error in verifying signature");
        }
    }
}
