using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewModels
{
    public class PaymentResponseVM
    {
        public string TransId { get; set; }
        public string ResponseCode { get; set; }
        public string CustomerId { get; set; }
        public string TotalAmount { get; set; }
        public string DateTime { get; set; }
    }
}
