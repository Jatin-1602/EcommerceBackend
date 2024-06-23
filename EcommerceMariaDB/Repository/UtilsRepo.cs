using EcommerceMariaDB.Data;
using System.IdentityModel.Tokens.Jwt;

namespace EcommerceMariaDB.Repository
{
    public class UtilsRepo
    {

        public static bool Save(DataContext db)
        {
            var saved = db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public static int GetDetails(string authHeader)
        {
            var _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = _jwtSecurityTokenHandler.ReadToken(authHeader);
            Console.WriteLine(jsonToken);

            var tokenS = _jwtSecurityTokenHandler.ReadToken(authHeader) as JwtSecurityToken;
            Console.WriteLine(tokenS);
            int userId = int.Parse(tokenS.Claims.First(c => c.Type == "id").Value);
            Console.WriteLine($"User id = {userId}");

            return userId;
        }
    }


}
