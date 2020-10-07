using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("Course")]
    public class CourseModel
    {
        public Int64 Id { get; set; }
        public Guid CloudId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string CourseId { get; set; }
        public string Description { get; set; }

    }
}
