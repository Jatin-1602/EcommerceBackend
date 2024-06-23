namespace EcommerceMariaDB.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int OrderId { get; set; }
    }
}
 