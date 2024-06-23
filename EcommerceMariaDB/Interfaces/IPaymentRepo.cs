namespace EcommerceMariaDB.Interfaces
{
    public interface IPaymentRepo
    {
        string Checkout(int orderId);
        bool VerifySignature(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int userId);
    }
}
