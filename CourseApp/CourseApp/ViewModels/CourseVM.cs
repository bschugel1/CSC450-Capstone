using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.ViewModels
{
    public class CourseVM
    {
        public Int64 Id { get; set; }
        public Guid CloudId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
    }
}
