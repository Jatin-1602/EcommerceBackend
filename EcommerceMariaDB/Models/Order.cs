using System.ComponentModel.DataAnnotations;

namespace EcommerceMariaDB.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string State {  get; set; } = string.Empty;

        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "Invalid Pin Code")]
        public string PinCode { get; set; } = string.Empty;

        [RegularExpression("^[6-9]\\d{9}$", ErrorMessage = "Invalid Contact No.")]
        public string Contact { get; set; } = string.Empty;

        public string RazorpayPaymentId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
