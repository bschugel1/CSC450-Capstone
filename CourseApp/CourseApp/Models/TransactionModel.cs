﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    [Table("Transaction")]
    public class TransactionModel
    {       
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public long CourseId { get; set; }
        public long UserId { get; set; }
        public CourseModel Course { get; set; }
        public UserModel User { get; set; }

        // Amount in USD 
        public decimal Amount { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }

    }
}
