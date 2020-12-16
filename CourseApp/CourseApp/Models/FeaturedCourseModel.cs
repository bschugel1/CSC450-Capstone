using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    public class FeaturedCourseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CourseId { get; set; }
        public CourseModel Course { get; set; }
        public string Feature { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
