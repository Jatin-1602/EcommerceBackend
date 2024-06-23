namespace EcommerceMariaDB.Models.Dto
{
    public class CartItemDto
    {
        public required int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
