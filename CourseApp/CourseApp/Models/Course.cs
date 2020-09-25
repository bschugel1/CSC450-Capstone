using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CourseAppCloud.Models
{
    public class Course
    {
        public Int64 Id { get; set; }
        public Guid CloudId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string CourseId { get; set; }
        public string Description { get; set; }
    }
}
