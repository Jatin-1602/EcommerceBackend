using Microsoft.AspNetCore.Mvc;
using EcommerceMariaDB.Interfaces;
using Microsoft.AspNetCore.Authorization;
using EcommerceMariaDB.Models;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackOrderController : Controller
    {
        private readonly ITrackOrderRepo _trackOrderRepo;
        public TrackOrderController(ITrackOrderRepo trackOrderRepo)
        {
            _trackOrderRepo = trackOrderRepo;
        }

        [HttpPost("/update-status"), Authorize(Roles = "admin")]
        public ActionResult UpdateOrderItemStatus(int orderItemId, string status)
        {
            if (_trackOrderRepo.UpdateStatus(orderItemId, status))
                return Ok("Status successfully updated");

            return BadRequest("Error in updating status");
        }

        [HttpGet("/track-order/{orderItemId}"), Authorize]
        public IActionResult TrackOrder(int orderItemId)
        {
            TrackOrder? status = _trackOrderRepo.GetStatus(orderItemId);
            if (status == null)
                return BadRequest("Invalid Item ID");

            return Ok(status);
        }
    }
}
