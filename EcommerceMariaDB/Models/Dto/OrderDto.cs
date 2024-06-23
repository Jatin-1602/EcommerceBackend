namespace EcommerceMariaDB.Models.Dto
{
    public class OrderDto
    {
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PinCode { get; set; }
        public required string Contact { get; set; }
        public required string RazorpayPaymentId { get; set; }
    }
}
