using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("Section")]
    public class SectionModel
    {
        [Key]
        public long Id { get; set; }     
        public long CourseId { get; set; }
        public long? ParentSectionId { get; set; }   
        public CourseModel Course { get; set; }
        public string Name { get; set; }      
        public int DisplayOrder { get; set; }
        public ICollection<MediaItemModel> Items { get; set; }
    }
}