using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.Models
{
    public class MediaItemModel
    {
        [Key]
        public long Id { get; set; }
        public string MediaType { get; set; }
        public long SectionId { get; set; }
        public SectionModel Section { get; set; }
        public int DisplayOrder { get; set; }
    }
}
