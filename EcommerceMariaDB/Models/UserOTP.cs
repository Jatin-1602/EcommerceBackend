namespace EcommerceMariaDB.Models
{
    public class UserOTP
    {
        public int Id { get; set; }
        public string OTP { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int ResetCount { get; set; } = 0;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}


