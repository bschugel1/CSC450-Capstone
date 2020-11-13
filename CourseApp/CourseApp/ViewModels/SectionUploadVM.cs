using CourseApp.Models;

namespace CourseApp.ViewModels
{
    public class SectionUploadVM
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public long? ParentSectionId { get; set; }

        public string Type { get; set; }
        public MediaItemModel Items { get; set; }
    }
}
