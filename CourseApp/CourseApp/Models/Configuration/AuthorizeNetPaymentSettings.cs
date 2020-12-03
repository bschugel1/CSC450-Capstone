using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.Models.Configuration
{
    public class AuthorizeNetPaymentSettings
    {
        public string MerchantDisplay { get; set; }
        public string ApiLoginId { get; set; }
        public string TransactionKey { get; set; }
        public string IframeURL { get; set; }
    }
}
