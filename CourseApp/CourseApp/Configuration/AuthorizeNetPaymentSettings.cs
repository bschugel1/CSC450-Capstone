
namespace CourseApp.Configuration
{
    public class AuthorizeNetPaymentSettings
    {
        public string MerchantDisplay { get; set; }
        public string ApiLoginId { get; set; }
        public string TransactionKey { get; set; }
        public string IframeURL { get; set; }
        public string SignatureKey { get; set; }
        public string ReceiptUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
