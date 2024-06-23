using Microsoft.AspNetCore.Mvc;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceMariaDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("/register")]
        public IActionResult RegisterUser(UserDto user)
        {
            Console.WriteLine(user + " " + user.Name + " " + user.Email);

            //User already exists
            if (_userRepo.UserExists(user.Email))
                return StatusCode(409, "Email Exists");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            
            User newUser = new User();
            newUser.Name = user.Name;
            newUser.Email = user.Email;
            newUser.PasswordHash = passwordHash;
            if (user.Role != null)
                   newUser.Role = user.Role;

            if (_userRepo.RegisterUser(newUser))
                return Ok("Successfully Registered");

            return BadRequest("Server Error");
        }

        [HttpPost("/login")]
        public IActionResult Login(UserLoginDto user)
        {
            if (!_userRepo.UserExists(user.Email))
                return NotFound("Email not exists");

            if (!_userRepo.CheckPassword(user.Email, user.Password))
                return StatusCode(401, "Invalid Credentials");

            string token = _userRepo.Login(user.Email, user.Password);

            return Ok(token);
        }


        [HttpGet("/get-profile"), Authorize]
        public IActionResult GetProfile()
        {
            int userId = int.Parse(HttpContext.Items["userId"]?.ToString()!);
            User? user = _userRepo.GetProfile(userId);
            return Ok(user);
        }


        [HttpGet("/generate-otp")]
        public IActionResult GenerateOTP(string email)
        {
            if (!_userRepo.UserExists(email))
                return NotFound("Email not exists");

            string? otp = _userRepo.GenerateOTP(email);
            if (otp == null)
                return BadRequest("Error in saving otp to database");
            
            return Ok($"OTP : {otp} sent successfully");
        }

        [HttpPost("/reset-password")]
        public IActionResult ResetPassword(string otp, string email, string password)
        {
            if (!_userRepo.UserExists(email))
                return NotFound("Email not exists");

            bool? response = _userRepo.ResetPassword(otp, email, password);


            if (response == null)
                return BadRequest("Invalid OTP");
            else if (response == true)
                return Ok("Password has been updated successfully");

            return BadRequest("Error in resetting password");
        }
    }
}
