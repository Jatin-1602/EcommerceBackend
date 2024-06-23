namespace EcommerceMariaDB.Models
{
    public class SellerProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SellerId { get; set; }
    }
}
