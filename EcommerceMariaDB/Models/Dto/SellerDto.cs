namespace EcommerceMariaDB.Models.Dto
{
    public class SellerDto
    {
        public required string Name { get; set; }
        public required string Mobile { get; set; }
        public required string GST { get; set; }
        public required string PinCode { get; set; }
        public required string Address { get; set; }
    }
}
