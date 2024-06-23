using EcommerceMariaDB.Models;
using EcommerceMariaDB.Models.Dto;

namespace EcommerceMariaDB.Interfaces
{
    public interface IUserRepo
    {
        bool UserExists(string email);
        bool CheckPassword(string email, string password);
        bool RegisterUser(User user);
        string Login(string email, string passwd);
        User GetProfile(int userId);
        string? GenerateOTP(string email);
        bool? ResetPassword(string otp, string email, string password);
    }
}
