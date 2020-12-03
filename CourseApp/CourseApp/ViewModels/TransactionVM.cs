using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.ViewModels
{
    public class TransactionVM
    {
        public long CourseId { get; set; }
        public string Response { get; set; }
        public string SessionToken { get; set; }
    }
}
