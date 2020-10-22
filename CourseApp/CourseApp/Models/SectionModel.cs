using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("Section")]
    public class SectionModel<T>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CourseModel Course { get; set; }
        public List<SectionSegment> Segments { get; set; }
    }
}