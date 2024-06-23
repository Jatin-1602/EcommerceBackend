namespace EcommerceMariaDB.Models
{
    public class TrackOrder
    {
        public int Id { get; set; }
        public bool Ordered { get; set; }
        public bool Shipped { get; set; }
        public bool OutForDelivery { get; set; }
        public bool Delivered { get; set; }
        public int OrderItemId { get; set; }
        //public int OrderId { get; set; }
    }
}
