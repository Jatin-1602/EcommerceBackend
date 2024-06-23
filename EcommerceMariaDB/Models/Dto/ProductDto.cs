namespace EcommerceMariaDB.Models.Dto
{
    public class ProductDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required float Price { get; set; }
        public IFormFile? File { get; set; }
        public required int CategoryId { get; set; }
        public int? UnitCount { get; set; }
        public required int SellerId { get; set; }
        public bool? ForceAdd { get; set; }
    }
}
