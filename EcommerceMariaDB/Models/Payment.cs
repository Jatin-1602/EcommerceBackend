using System.ComponentModel.DataAnnotations;

namespace EcommerceMariaDB.Models
{
    public class Payment
    {
        [Key]
        public string Id { get; set; }
        public string RazorpayOrderId { get; set; }
        public int UserId { get; set; }
    }
}
