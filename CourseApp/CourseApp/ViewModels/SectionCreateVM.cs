
namespace CourseApp.ViewModels
{
    public class SectionCreateVM
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public long? ParentSectionId { get; set; }     
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
