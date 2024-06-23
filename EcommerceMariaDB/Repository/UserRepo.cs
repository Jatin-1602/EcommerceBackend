using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceMariaDB.Repository
{
    public class UserRepo : IUserRepo
    {
        private string _generatedOTP = "";
        private readonly DataContext _db;
        private readonly IEmailRepo _emailRepo;
        private readonly IConfiguration _configuration;

        public UserRepo(DataContext db, IConfiguration configuration, IEmailRepo emailRepo)
        {
            _db = db;
            _configuration = configuration;
            _emailRepo = emailRepo;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public User? GetUser(string email)
        {
            User? user = _db.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public bool UserExists(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        public bool CheckPassword(string email, string password)
        {
            User? user = GetUser(email);

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return false;
            return true;
        }

        public bool RegisterUser(User user)
        {
            _db.Users.Add(user);
            if (!UtilsRepo.Save(_db))
                return false;

            // Creating cart for each new user (role = consumer)
            if (user.Role == "consumer")
            {
                Cart cart = new Cart();
                cart.UserId = user.Id;
                _db.Carts.Add(cart);
            }
            else
                return true;

            return UtilsRepo.Save(_db);
        }

        public string Login(string email, string password)
        {
            User? user = GetUser(email);
            string token = CreateToken(user);
            return token;
        }

        public User GetProfile(int userId)
        {
            User? user = _db.Users.Find(userId);
            return user;
        }

        public string? GenerateOTP(string email)
        {
            User? user = GetUser(email);
            int userId = user.Id;
            string otp = RandomNumberGenerator.GetInt32(1, 1000000).ToString("D6");
            _generatedOTP = otp;
            Console.WriteLine(otp);

            Email mail = new Email();
            mail.To = email;
            mail.Subject = "OTP for Reset Password";
            mail.Body = $"Your otp for resetting the password is {otp}.\nThis otp is valid only for 2 minutes.";
            _emailRepo.SendEmail(mail);

            if (!SaveOTPValue(userId, otp))
                return null;

            return otp;
        }

        public bool? ResetPassword(string otp, string email, string password)
        {
            User? user = GetUser(email);
            UserOTP? userExists = _db.UserOTPs.Where(uo => uo.UserId == user.Id).First();

            //Invalid OTP
            if (otp != userExists.OTP || userExists.CreateTime.AddSeconds(60) < DateTime.Now)
                return null;

            //Update User Password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = passwordHash;
            user.ModifiedAt = DateTime.Now;
            _db.Users.Update(user);

            //Update Reset Count in UserOTPs
            userExists.ResetCount += 1;
            _db.UserOTPs.Update(userExists);

            return UtilsRepo.Save(_db);
        }

        private bool SaveOTPValue(int userId, string otp)
        {
            UserOTP? userExists = _db.UserOTPs.Where(uo => uo.UserId == userId).FirstOrDefault();
            if (userExists == null)
            {
                UserOTP userOTP = new UserOTP();
                userOTP.OTP = otp;
                userOTP.UserId = userId;

                _db.UserOTPs.Add(userOTP);
            }
            else
            {
                userExists.OTP = otp;
                userExists.CreateTime = DateTime.Now;

                _db.UserOTPs.Update(userExists);
            }

            return UtilsRepo.Save(_db);
        }
    }
}
