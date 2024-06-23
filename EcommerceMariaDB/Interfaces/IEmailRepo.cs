using EcommerceMariaDB.Models;

namespace EcommerceMariaDB.Interfaces
{
    public interface IEmailRepo
    {
        void SendEmail(Email request);
    }
}
