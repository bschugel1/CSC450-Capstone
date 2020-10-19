using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("Course")]
    public class CourseModel
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public UserModel Author { get; set; }
    }
}
