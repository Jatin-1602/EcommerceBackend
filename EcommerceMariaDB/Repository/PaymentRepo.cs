using EcommerceMariaDB.Data;
using EcommerceMariaDB.Interfaces;
using EcommerceMariaDB.Models;


//using EcommerceMariaDB.Models;
using Razorpay.Api;

namespace EcommerceMariaDB.Repository
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly DataContext _db;
        private readonly IConfiguration _configuration;
        private RazorpayClient _client;
        public PaymentRepo(DataContext db, IConfiguration configuration)
        {

            _db = db;
            _configuration = configuration;
            _client = new RazorpayClient(_configuration["Razorpay:key"], _configuration["Razorpay:secret"]);
        }

        public string Checkout(int userId)
        {
            List<CartItem> items = (from cart in _db.Carts
                                    where cart.UserId == userId
                                    join item in _db.CartItems
                                    on cart.Id equals item.CartId
                                    select item).ToList();

            float amount = items.Sum(item => item.Amount);
            Console.WriteLine("Amount = " + amount);

            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", Convert.ToDecimal(amount) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", "12121");

            //RazorpayClient client = new RazorpayClient(key, secret);

            string? razorpay_order_id = null;
            Razorpay.Api.Order razorpay_order = _client.Order.Create(input);
            razorpay_order_id = razorpay_order["id"].ToString();

            return razorpay_order_id;
        }

        public bool VerifySignature(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int userId)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            attributes.Add("razorpay_payment_id", razorpay_payment_id);
            attributes.Add("razorpay_order_id", razorpay_order_id);
            attributes.Add("razorpay_signature", razorpay_signature);

            try
            {
                //Utils.verifyPaymentSignature(attributes);
                string s = "panoti";
            }
            catch (Exception ex)
            {
                return false;
            }

            //Add payment details to Payment Table
            Models.Payment payment = new Models.Payment();
            payment.Id = razorpay_payment_id;
            payment.RazorpayOrderId = razorpay_order_id;
            payment.UserId = userId;

            _db.Payments.Add(payment);

            return UtilsRepo.Save(_db);
        }
    }
}
