using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController: Controller
    {
        private readonly IEmailRepo _emailRepo;
        public EmailController(IEmailRepo emailRepo)
        {
            _emailRepo = emailRepo;
        }

        [HttpPost("/send-mail")]
        public IActionResult SendEmail(Email request)
        {
            _emailRepo.SendEmail(request);
            return Ok("Email sent successfully");
        }
    }
}
