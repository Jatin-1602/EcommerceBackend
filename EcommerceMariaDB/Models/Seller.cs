namespace EcommerceMariaDB.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string GST { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int UserId {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}
