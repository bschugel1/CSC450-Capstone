using CourseApp.Models;
using System.Collections.Generic;

namespace CourseApp.ViewModels
{
    public class SectionVM
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public long? ParentSectionId { get; set; }
        public CourseModel Course { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<MediaItemModel> Items { get; set; }
    }
}
