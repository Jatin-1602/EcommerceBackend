namespace EcommerceMariaDB.Models
{
    public class SellerOrder
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int SellerId { get; set; }
    }
}
