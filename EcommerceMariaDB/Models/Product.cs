using System.Reflection.Metadata;

namespace EcommerceMariaDB.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public byte[]? Image { get; set; }
        public int UnitCount { get; set; } = 0;
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}
