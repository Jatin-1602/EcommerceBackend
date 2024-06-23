namespace EcommerceMariaDB.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Amount { get; set; }
        public int CartId { get; set; }
    }
}
